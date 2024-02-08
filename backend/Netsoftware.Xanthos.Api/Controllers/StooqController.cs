using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netsoftware.Xanthos.Api.Services;
using Netsoftware.Xanthos.Common.HttpClient;
using Netsoftware.Xanthos.Infrastructure.Dto;
using Newtonsoft.Json;
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
			const string versionNumber = "9.61";
			return versionNumber;
		}

		[AllowAnonymous]
		[HttpGet("GetStockData")]
		public async Task<List<StooqModel>> GetStockDataAsync()
		{
			string companyPrefix = "pkn";
			string APIurl = "https://stooq.pl/q/d/l/?s=" + companyPrefix + "&i=d";
			
			var stooqResponse = await _httpClientApiService.GetRaw(APIurl,false);
			var result = await stooqResponse.Content.ReadAsStringAsync();
			var chartData = _stooqService.ParseCsvData(result);

			return chartData;
		}
	}
}
