using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Netsoftware.Xanthos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public string Get()
    {
        const string versionNumber = "1.1";
        return versionNumber;
    }

    [Authorize(Roles = "SuperAdmin, InstanceAdmin")]
    [HttpGet("GetAuth")]
    public bool GetAuthentication()
    {
        return true;
    }
}