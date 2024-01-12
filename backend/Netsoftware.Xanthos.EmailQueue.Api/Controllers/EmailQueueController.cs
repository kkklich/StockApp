using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.EmailSender;
using Netsoftware.Xanthos.EmailQueue.Infrastructure.Services.Interfaces;

namespace Netsoftware.Xanthos.EmailQueue.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class EmailQueueController : ControllerBase
{
    private readonly IEmailQueueService _emailQueueService;
    private readonly ILogger _logger;

    public EmailQueueController(ILogger<EmailQueueController> logger, IEmailQueueService emailQueueService)
    {
        _logger = logger;
        _emailQueueService = emailQueueService;
    }

    [HttpPost]
    public async Task<IActionResult> AddEmailToQueue([FromBody] EmailQueueResource data)
    {
        try
        {
            _logger.LogInformation("Start saving email for send");
            await _emailQueueService.AddEmailToQueue(data);
            _logger.LogInformation("End saving email for send");

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method AddEmailToQueue()", data);
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEmailsAboveEmailId([FromQuery] int id)
    {
        try
        {
            _logger.LogInformation("Start GetEmailsAboveEmailId");
            var result = await _emailQueueService.GetEmailsAboveEmailId(id);
            _logger.LogInformation("End GetEmailsAboveEmailId");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method GetEmailsAboveEmailId()", id);
            return BadRequest();
        }
    }
}