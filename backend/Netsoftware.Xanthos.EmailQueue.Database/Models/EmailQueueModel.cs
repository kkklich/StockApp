using System;
using Netsoftware.Xanthos.Common.EmailSender.Enumerators;
using Netsoftware.Xanthos.Common.Resources.Interfaces;

namespace Netsoftware.Xanthos.EmailQueue.Database.Models;

public class EmailQueueModel : IDeletable, IApplicationInstance
{
    public EmailQueueModel()
    {
        EmailStatus = EmailStatus.NotSend;
        ErrorsCount = 0;
        IsDelete = false;
    }

    public int Id { get; set; }
    public EmailStatus EmailStatus { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public int ErrorsCount { get; set; }
    public string Receivers { get; set; }
    public string ErrorDetails { get; set; }
    public string FileDownloadUrls { get; set; }
#nullable enable
    public DateTime? ModificationDate { get; set; }
#nullable disable
    public Guid AppId { get; set; }
    public bool IsDelete { get; set; }
}