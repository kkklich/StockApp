using System.Collections.Generic;
using System.Linq;
using System;
using Netsoftware.Xanthos.Infrastructure.Dto;
using System.Drawing;
using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Api.Services
{
	public class StooqService
	{
		public StooqService() { }

		public List<StooqModel> ParseCsvData(string csvData)
		{
			var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			var stockDataList = new List<StooqModel>();

			foreach (var line in lines.Skip(1)) // Skip header row
			{
				var values = line.Split(',');
				var stockData = new StooqModel
				{
					Date = DateTime.Parse(values[0]),
					Open = double.Parse(values[1].Replace('.', ',')),
					High = double.Parse(values[2].Replace('.', ',')),
					Low = double.Parse(values[3].Replace('.', ',')),
					Close = double.Parse(values[4].Replace('.', ',')),
					Volumen = double.Parse(values[5].Replace('.', ','))
				};

				stockDataList.Add(stockData);
			}

			return stockDataList;
		}

		public async Task<List<StooqModel>> GetDataFromAPI(string ticker, string period)
		{

			return null;
		}
	}
}
