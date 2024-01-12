namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailResource
{
    public EmailResource()
    {
        Address = new EmailAddress();
    }

    public string Subject { get; set; }
    public string Content { get; set; }
    public EmailAddress Address { get; set; }
}