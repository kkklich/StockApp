using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Common.EmailSender.Interfaces;

public interface IEmailService
{
    void Send(EmailMessage emailMessage, EmailAddress replayEmail = null, List<EmailFile> files = null);
    List<EmailMessage> ReceiveEmail(int maxCount = 10);
    Task AddEmailToQueue(EmailQueueResource data);
}