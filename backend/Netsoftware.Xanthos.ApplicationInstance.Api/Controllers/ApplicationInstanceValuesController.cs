using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Netsoftware.Xanthos.ApplicationInstance.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationInstanceValuesController : ControllerBase
{
    private readonly ILogger _logger;

    public ApplicationInstanceValuesController(ILogger<ApplicationInstanceValuesController> logger)
    {
        _logger = logger;
    }

    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        _logger.LogInformation("Test Log");
        return new[] { "ApplicationInstance 1 ", "ApplicationInstance 2" };
    }


    // GET api/values/5
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
        return "ApplicationInstance";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}