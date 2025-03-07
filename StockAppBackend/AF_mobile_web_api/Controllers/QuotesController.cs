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

        [HttpGet("GetStockData")]
        public async Task<IActionResult> GetStockData([FromQuery] string prefix, [FromQuery] string interval)
        {
            try
            {
                var xd = new { message = prefix.ToString()+ " " +interval.ToString() };
                return Ok(xd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
