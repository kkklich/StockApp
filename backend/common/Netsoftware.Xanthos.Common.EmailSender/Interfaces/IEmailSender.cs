using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Common.EmailSender.Interfaces;

public interface IEmailSender
{
    EmailAddress CreateEmailAddress(string name, string address);
    EmailMessage CreateEmailMessage(string subject, string content, EmailAddress receiverAddress);
    EmailMessage CreateEmailMessage(string subject, string content, List<EmailAddress> receiverAddress);
    void Send(EmailMessage emailMessage, EmailAddress replayEmail = null, List<EmailFile> files = null);
    List<EmailMessage> ReceiveEmail(int maxCount = 10);
    Task AddEmailToQueue(EmailQueueResource data);
}