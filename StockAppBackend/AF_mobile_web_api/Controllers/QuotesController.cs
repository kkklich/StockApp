using AF_mobile_web_api_Application.DTO;
using AF_mobile_web_api_Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AF_mobile_web_api.Controllers
{

    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private IQuotesServices _quotesServices;

        public QuotesController(IQuotesServices quotesServices)
        {
            _quotesServices = quotesServices;
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
                var quotes = await _quotesServices.GetStockData(prefix, interval);
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetStockIndicators")]
        public async Task<IActionResult> GetStockIndicators([FromQuery] string prefix, [FromQuery] string interval, [FromQuery] int period)
        {
            try
            {
                var quotes = await _quotesServices.GetStockEMA(prefix, interval, period);
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("BackgroundStock")]
        public async Task<IActionResult> CheckBackgroundStock([FromQuery] string prefix, [FromQuery] string interval, [FromQuery] string date)
        {
            try
            {
                var quotes = await _quotesServices.CheckBackground(prefix, interval, date);
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
