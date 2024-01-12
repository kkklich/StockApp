namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailConfiguration
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }

    public string PopServer { get; set; }
    public int PopPort { get; set; }
    public string PopUsername { get; set; }
    public string PopPassword { get; set; }

    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public bool TLS { get; set; }
}