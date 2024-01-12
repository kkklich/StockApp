using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Netsoftware.Xanthos.Common.EmailSender;
using Netsoftware.Xanthos.EmailQueue.Database.Models;
using Netsoftware.Xanthos.EmailQueue.Database.Repositories;
using Netsoftware.Xanthos.EmailQueue.Infrastructure.Services.Interfaces;

namespace Netsoftware.Xanthos.EmailQueue.Infrastructure.Services;

public class EmailQueueService : IEmailQueueService
{
    private readonly IGenericRepository<EmailQueueModel> _emailQueueRepository;
    private readonly IMapper _mapper;

    public EmailQueueService(IMapper mapper, IGenericRepository<EmailQueueModel> emailQueueRepository)
    {
        _mapper = mapper;
        _emailQueueRepository = emailQueueRepository;
    }

    public async Task AddEmailToQueue(EmailQueueResource data)
    {
        var email = _mapper.Map(data, new EmailQueueModel());

        await _emailQueueRepository.CreateAsync(email);
    }

    public async Task<List<EmailQueueResource>> GetEmailsAboveEmailId(int id)
    {
        return (await _emailQueueRepository.GetAsync(x => x.Id > id))
            .Select(x => _mapper.Map<EmailQueueResource>(x))
            .ToList();
    }
}