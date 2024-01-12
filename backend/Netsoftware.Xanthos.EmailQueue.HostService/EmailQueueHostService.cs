using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.EmailSender;
using Netsoftware.Xanthos.Common.EmailSender.Enumerators;
using Netsoftware.Xanthos.Common.EmailSender.Interfaces;
using Netsoftware.Xanthos.Common.HttpClient;
using Netsoftware.Xanthos.Database;

namespace Netsoftware.Xanthos.EmailQueue.HostService;

public class EmailQueueHostService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailQueueHostService> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public EmailQueueHostService(ILogger<EmailQueueHostService> logger, IServiceProvider serviceProvider,
        IMapper mapper, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Convert.ToBoolean(_configuration.GetSection("RunBackgroundTasks").Value)) return;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendEmails();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error during sending emails");
            }

            await Task.Delay(new TimeSpan(0, 1, 0), stoppingToken);
        }
    }

    private async Task SendEmails()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var emailsForSend = await context.EmailQueues
            .Where(x => !x.IsDelete && (x.EmailStatus == EmailStatus.NotSend || x.EmailStatus == EmailStatus.Error))
            .ToListAsync();

        if (!emailsForSend.Any()) return;

        foreach (var email in emailsForSend)
            try
            {
                var emailResource = _mapper.Map<EmailQueueResource>(email);
                if (emailResource.Receivers.Count == 0)
                    throw new InvalidOperationException("Receivers list cannot be empty");

                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var message = emailSender.CreateEmailMessage(emailResource.Subject, emailResource.Content,
                    emailResource.Receivers);

                if (email.FileDownloadUrls != null)
                {
                    var httpService = scope.ServiceProvider.GetRequiredService<IHttpClientApiService>();
                    emailSender.Send(message, null,
                        await CreateEmailFiles(emailResource.FileDownloadUrls, httpService));
                }
                else
                {
                    emailSender.Send(message);
                }

                email.EmailStatus = EmailStatus.Send;
                email.IsDelete = true;
                email.ModificationDate = DateTime.UtcNow;

                _logger.LogInformation("Email has been send", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sending email", email);
                email.EmailStatus = EmailStatus.Error;
                email.ErrorsCount++;
                email.ErrorDetails = ex.Message;
                email.ModificationDate = DateTime.UtcNow;

                if (email.ErrorsCount >= 10) email.IsDelete = true;
            }

        context.UpdateRange(emailsForSend);
        await context.SaveChangesAsync();
    }

    private async Task<List<EmailFile>> CreateEmailFiles(List<string> fileDownloadUrls,
        IHttpClientApiService httpService)
    {
        var result = new List<EmailFile>();

        foreach (var path in fileDownloadUrls) result.Add(await CreateEmailFile(path, httpService));

        return result;
    }

    private async Task<EmailFile> CreateEmailFile(string url, IHttpClientApiService httpService)
    {
        var file = await httpService.GetFile(url, false);
        return new EmailFile(file.FileDownloadName, file.FileContents);
    }
}