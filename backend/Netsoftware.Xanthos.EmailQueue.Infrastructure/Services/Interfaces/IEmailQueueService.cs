using System.Collections.Generic;
using System.Threading.Tasks;
using Netsoftware.Xanthos.Common.EmailSender;

namespace Netsoftware.Xanthos.EmailQueue.Infrastructure.Services.Interfaces;

public interface IEmailQueueService
{
    Task AddEmailToQueue(EmailQueueResource data);
    Task<List<EmailQueueResource>> GetEmailsAboveEmailId(int id);
}