using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Services.Interfaces;
using Netsoftware.Xanthos.Common.Resources.Account;
using Netsoftware.Xanthos.Common.Resources.GridResources;
using Newtonsoft.Json;

namespace Netsoftware.Xanthos.ApplicationInstance.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ApplicationInstancesController : ControllerBase
{
    private readonly IApplicationInstancesService _applicationInstancesService;
    private readonly ILogger _logger;

    public ApplicationInstancesController(ILogger<ApplicationInstancesController> logger,
        IApplicationInstancesService applicationInstancesService)
    {
        _logger = logger;
        _applicationInstancesService = applicationInstancesService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterAccountResource resource)
    {
        try
        {
            await _applicationInstancesService.Register(resource);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute method Register().", resource);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetApplicationInstancesTableData([FromQuery] string paramsJSON)
    {
        try
        {
            var gridParams = JsonConvert.DeserializeObject<GridParamsResource>(paramsJSON);

            _logger.LogInformation("Start getting delivery packages");
            var result = await _applicationInstancesService.GetApplicationInstancesTableData(gridParams);
            _logger.LogInformation("End getting delivery packages");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method GetApplicationInstancesTableData()", paramsJSON);
            return BadRequest(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetApplicationInstance([FromQuery] Guid id)
    {
        try
        {
            _logger.LogInformation("Start GetApplicationInstance()", id);
            var result = await _applicationInstancesService.GetApplicationInstance(id);
            _logger.LogInformation("End GetTrackedDocument()", id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execute method GetApplicationInstance()", id);
            return BadRequest(ex);
        }
    }
}