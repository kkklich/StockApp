using Netsoftware.Xanthos.Common.EmailSender.Interfaces;

namespace Netsoftware.Xanthos.Common.EmailSender.EmailTemplates;

public class EmailModel : IEmailTemplateModel
{
    public EmailModel(string receiver, string sender, string content, string url, string projectName)
    {
        Receiver = receiver;
        Sender = sender;
        Content = content;
        Url = url;
        ProjectName = projectName;
	}

    public string Receiver { get; set; }
    public string Sender { get; set; }
    public string Content { get; set; }
    public string Url { get; set; }
    public string ProjectName { get; set; }
}