using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Providers.Interfaces;

namespace Netsoftware.Xanthos.ApplicationInstance.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ExternalDataController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IUsersDataProvider _usersDataProvider;

    public ExternalDataController(ILogger<ExternalDataController> logger, IUsersDataProvider usersDataProvider)
    {
        _logger = logger;
        _usersDataProvider = usersDataProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersByApplicationInstanceTableData([FromQuery] string paramsJSON,
        [FromQuery] Guid applicationInstanceId)
    {
        try
        {
            var resultJSON =
                await _usersDataProvider.DownloadApplicationInstanceUsers(paramsJSON, applicationInstanceId);
            return Ok(resultJSON);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method GetUsersByApplicationInstanceTableData()");
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetApplicationInstanceOwnersList()
    {
        try
        {
            var resultJSON = await _usersDataProvider.DownloadApplicationInstancesOwners();
            return Ok(resultJSON);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method GetApplicationInstanceOwnersList()");
            return BadRequest();
        }
    }
}