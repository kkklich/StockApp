using System;
using System.Collections.Generic;
using Netsoftware.Xanthos.Common.EmailSender.Enumerators;

namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailQueueResource
{
    public int Id { get; set; }
    public Guid AppId { get; set; }
    public List<EmailAddress> Receivers { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public EmailStatus EmailStatus { get; set; }
    public int ErrorsCount { get; set; }
    public bool IsDelete { get; set; }
    public string ErrorDetails { get; set; }
#nullable enable
    public DateTime? ModificationDate { get; set; }
    public List<string>? FileDownloadUrls { get; set; }
#nullable disable
}