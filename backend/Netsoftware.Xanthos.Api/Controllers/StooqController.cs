using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netsoftware.Xanthos.Api.Services;
using Netsoftware.Xanthos.Common.HttpClient;
using Netsoftware.Xanthos.Infrastructure.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StooqController : Controller
	{

		private readonly IHttpClientApiService _httpClientApiService;
		private readonly StooqService _stooqService;

		public StooqController(IHttpClientApiService httpClientApiService, StooqService stooqService)
		{
			_httpClientApiService = httpClientApiService;
			_stooqService = stooqService;
		}

		[AllowAnonymous]
		[HttpGet]
		public string Get()
		{
			const string versionNumber = "0.01";
			return versionNumber;
		}

		[AllowAnonymous]
		[HttpPost("GetStockData")]
		public async Task<List<StooqModel>> GetStockDataAsync([FromBody] QuotesModel quotes)
		{
			//string companyPrefix = "pkn";
			//string APIurl = "https://stooq.pl/q/d/l/?s=" + companyPrefix + "&i=d";
			string APIurl = "https://stooq.pl/q/d/l/?s=" + quotes.Prefix + "&i=" + quotes.Interval + "";
			
			var stooqResponse = await _httpClientApiService.GetRaw(APIurl,false);
			var result = await stooqResponse.Content.ReadAsStringAsync();
			var chartData = _stooqService.ParseCsvData(result);

			return chartData;
		}

		[HttpGet("GetStock/{ticker}/{period}")]
		public async Task<IActionResult> GetStock(string ticker, string period)
		{
			try
			{
				var data = await _stooqService.GetDataFromAPI(ticker, period);
				return Ok(data);
			}catch(Exception f)			
			{
				return BadRequest(f.Message);
			}
		}
	}
}
