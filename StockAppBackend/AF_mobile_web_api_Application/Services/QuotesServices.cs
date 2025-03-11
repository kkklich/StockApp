using AF_mobile_web_api.Helpers;
using AF_mobile_web_api_Application.DTO;
using AF_mobile_web_api_Application.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.Services
{
    public class QuotesServices: IQuotesServices
    {
        private readonly HttpClient _httpClient;
        private readonly IIndicatorsServices _indicatorsServices;
        public QuotesServices(HttpClient httpClient, IIndicatorsServices indicatorsServices) 
        {
            _httpClient = httpClient;
            _indicatorsServices = indicatorsServices;
        }

        public async Task<List<TwistData>> GetStockData(string symbol, string zoom)
        {
            try
            {
                var url = BuildUrlStockTwist(symbol, zoom);
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var stockDataList = JsonConvert.DeserializeObject<List<TwistData>>(jsonResponse);

                return stockDataList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<StockDataIndicators>> GetStockEMA(string symbol, string zoom, int period)
        {
            try
            {
                var stockData = await GetStockData(symbol, zoom);
                var closePrices = stockData.Select(x => x.Last).ToList();
                var ema = _indicatorsServices.CalculateEma(closePrices, period);

                var stockDataWithIndicators = new List<StockDataIndicators>();

                for (int i = 0; i < stockData.Count; i++)
                {
                    stockDataWithIndicators.Add(new StockDataIndicators
                    {
                        High = stockData[i].High,
                        Last = stockData[i].Last,
                        Volume = stockData[i].Volume,
                        Tm = stockData[i].Tm,
                        Low = stockData[i].Low,
                        Date = stockData[i].Date,
                        Open = stockData[i].Open,
                        EMA= i >= period - 1 ? ema[i - (period - 1)] : 0,
                    });
                }

                return stockDataWithIndicators;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<StockDataIndicators> CheckBackground(string symbol, string zoom, string dateString)
        {
            try
            {
                int period = 20;
                DateTime date;
                if (!DateTime.TryParse(dateString, out DateTime convertDate))
                    return null;                    
                
                date = DateTime.Parse(dateString);
                var indicatorStock = await GetStockEMA(symbol, zoom, period);
                //var isUp = _indicatorsServices.IsBackgroundUpAdvance(indicatorStock, date, period);
                var isUp = _indicatorsServices.IsBackgroundUp(indicatorStock, date, period);
                
                var targetDay = indicatorStock.FirstOrDefault(d => d.Date.Date == date.Date);
                if(targetDay != null) 
                    targetDay.IsBackgroundUp = isUp;

                return targetDay ?? new StockDataIndicators();
            }
            catch
            {
                throw;
            }
        }

        private string BuildUrlStockTwist(string symbol, string zoom)
        {
            var uriBuilder = new UriBuilder(ConstantHelper.StockTwist);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["symbol"] = symbol;
            query["zoom"] = zoom;
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }
    }
}
