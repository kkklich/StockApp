using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Netsoftware.Xanthos.Common.EmailSender.Interfaces;
using Netsoftware.Xanthos.Common.HttpClient;
using Netsoftware.Xanthos.Common.HttpClient.UrlProvider;
using Newtonsoft.Json;

namespace Netsoftware.Xanthos.Common.EmailSender;

internal class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly IHttpClientApiService _httpClientApiService;
    private readonly IUrlProvider _urlProvider;

    public EmailService(IOptions<EmailConfiguration> configuration, IUrlProvider urlProvider,
        IHttpClientApiService httpClientApiService)
    {
        _emailConfiguration = configuration.Value;
        _urlProvider = urlProvider;
        _httpClientApiService = httpClientApiService;
    }

    public List<EmailMessage> ReceiveEmail(int maxCount = 10)
    {
        using (var emailClient = new Pop3Client())
        {
            emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

            emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

            var emails = new List<EmailMessage>();
            for (var i = 0; i < emailClient.Count && i < maxCount; i++)
            {
                var message = emailClient.GetMessage(i);
                var emailMessage = new EmailMessage
                {
                    Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                    Subject = message.Subject
                };
                emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x)
                    .Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x)
                    .Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                emails.Add(emailMessage);
            }

            return emails;
        }
    }

    public void Send(EmailMessage emailMessage, EmailAddress replayEmail = null, List<EmailFile> files = null)
    {
        var message = new MimeMessage();
        message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
        message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

        message.Subject = emailMessage.Subject;

        if (replayEmail != null) message.ReplyTo.Add(new MailboxAddress(replayEmail.Name, replayEmail.Address));

        var builder = new BodyBuilder
        {
            HtmlBody = emailMessage.Content
        };

        if (files != null)
            foreach (var file in files)
                builder.Attachments.Add(file.Name, file.FileData);

        message.Body = builder.ToMessageBody();

        using (var emailClient = new SmtpClient())
        {
            if (_emailConfiguration.TLS)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort,
                    SecureSocketOptions.StartTls);
            else
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort,
                    SecureSocketOptions.None);

            emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

            emailClient.Send(message);

            emailClient.Disconnect(true);
        }
    }

    public async Task AddEmailToQueue(EmailQueueResource data)
    {
        var bodyJSON = JsonConvert.SerializeObject(data);
        await _httpClientApiService.Post<object>(
            _urlProvider.GetUrl("AppstationApi") + "/api/EmailQueue/AddEmailToQueue", bodyJSON);
    }
}