using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailMessage
{
    public EmailMessage()
    {
        ToAddresses = new List<EmailAddress>();
        FromAddresses = new List<EmailAddress>();
    }

    public EmailMessage(string senderName, string senderEmail, string subject, string content,
        EmailAddress receiverAddress)
    {
        Subject = subject;
        Content = content;
        ToAddresses = new List<EmailAddress>();
        FromAddresses = new List<EmailAddress>();

        ToAddresses.Add(receiverAddress);
        FromAddresses.Add(new EmailAddress(senderName, senderEmail));
    }

    public EmailMessage(string senderName, string senderEmail, string subject, string content,
        List<EmailAddress> receiverAddress)
    {
        Subject = subject;
        Content = content;

        ToAddresses = receiverAddress;
        FromAddresses = new List<EmailAddress>
        {
            new(senderName, senderEmail)
        };
    }

    public List<EmailAddress> ToAddresses { get; set; }
    public List<EmailAddress> FromAddresses { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}