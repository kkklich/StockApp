using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netsoftware.Xanthos.Common.EmailSender.Interfaces;

namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    public EmailSender(IEmailService emailService, IOptions<EmailConfiguration> configuration,
        ILogger<EmailSender> logger)
    {
        _emailService = emailService;
        _emailConfig = configuration.Value;
        _logger = logger;
    }

    public EmailAddress CreateEmailAddress(string name, string address)
    {
        return new EmailAddress(name, address);
    }

    public EmailMessage CreateEmailMessage(string subject, string content, EmailAddress receiverAddress)
    {
        return new EmailMessage(_emailConfig.SenderName, _emailConfig.SenderEmail, subject, content, receiverAddress);
    }

    public EmailMessage CreateEmailMessage(string subject, string content, List<EmailAddress> receiverAddress)
    {
        return new EmailMessage(_emailConfig.SenderName, _emailConfig.SenderEmail, subject, content, receiverAddress);
    }

    public List<EmailMessage> ReceiveEmail(int maxCount = 10)
    {
        try
        {
            return _emailService.ReceiveEmail(maxCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method ReceiveEmail()", maxCount);
            throw;
        }
    }

    public void Send(EmailMessage emailMessage, EmailAddress replayEmail = null, List<EmailFile> files = null)
    {
        try
        {
            _emailService.Send(emailMessage, replayEmail, files);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method Send()", emailMessage, replayEmail, files);
            throw;
        }
    }

    public async Task AddEmailToQueue(EmailQueueResource data)
    {
        try
        {
            await _emailService.AddEmailToQueue(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method AddEmailToQueue()", data);
            throw;
        }
    }
}