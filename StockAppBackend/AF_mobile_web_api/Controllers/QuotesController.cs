using Microsoft.AspNetCore.Mvc;

namespace AF_mobile_web_api.Controllers
{

    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {

        public QuotesController()
        {
        }

        [HttpGet("test")]
        public async Task<IActionResult> testAPI()
        {
            return Ok("work");
        }
    }    
}
