using AF_mobile_web_api.Helpers;
using AF_mobile_web_api_Application.DTO;
using AF_mobile_web_api_Application.DTO.Enums;
using AF_mobile_web_api_Application.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
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

        public async Task<List<StockDataIndicators>> GetStockData(string symbol, string zoom, string from = "", string to = "")
        {
            try
            {
                //var url = BuildUrlStockTwist(symbol, zoom); //TODO TWIST
                var url = BuildUrlStockWatch(symbol, zoom, from, to);
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                //var stockDataList = JsonConvert.DeserializeObject<List<TwistData>>(jsonResponse); TODO  //ConvertToStockDataIndicators
                var stockDataList = JsonConvert.DeserializeObject<StockWatch>(jsonResponse);
                var convertedData = ConvertStockWatch(stockDataList);

                return convertedData;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private List<StockDataIndicators> ConvertStockWatch(StockWatch stockData)
        {
            var stockDataIndicators = new List<StockDataIndicators>();
            for (int i = 0; i < stockData.Quotes.Count; i++)
            {
                stockDataIndicators.Add(new StockDataIndicators
                {
                    High = stockData.Quotes[i].High ,
                    Close = stockData.Quotes[i].Close,
                    Volume = stockData.Volume.FirstOrDefault(x => x.Time == stockData.Quotes[i].Time)?.Value ?? 0,                    
                    Low = stockData.Quotes[i].Low,
                    Date = stockData.Quotes[i].Time,
                    Open = stockData.Quotes[i].Open,
                    EMA = 0,
                });
            }
            return stockDataIndicators;
        }


        private List<StockDataIndicators> ConvertTwist(List<TwistData> stockData)
        {
            var stockDataIndicators = new List<StockDataIndicators>();
            for (int i = 0; i < stockData.Count; i++)
            {
                stockDataIndicators.Add(new StockDataIndicators
                {
                    High = stockData[i].High,
                    Close = stockData[i].Last,
                    Volume = stockData[i].Volume,
                    Low = stockData[i].Low,
                    Date = stockData[i].Date,
                    Open = stockData[i].Open,
                    EMA =  0,
                });
            }
            return stockDataIndicators;
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
        private string BuildUrlStockWatch(string symbol, string zoom, string from, string to)
        {
            var uriBuilder = new UriBuilder(ConstantHelper.StockWacth);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["Symbol"] = symbol;
            query["Resolution"] = zoom;
            query["From"] = from;
            query["To"] = to;
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        public async Task<List<StockDataIndicators>> GetStockEMA(string symbol, string zoom, int period)
        {
            try
            {
                var stockData = await GetStockData(symbol, zoom, "","");
                var closePrices = stockData.Select(x => x.Close).ToList();
                var ema = _indicatorsServices.CalculateEma(closePrices, period);

                var stockDataWithIndicators = new List<StockDataIndicators>();

                for (int i = 0; i < stockData.Count; i++)
                {
                    stockDataWithIndicators.Add(new StockDataIndicators
                    {
                        High = stockData[i].High,
                        Close = stockData[i].Close,
                        Volume = stockData[i].Volume,
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
                throw new ArgumentException(ex.Message);
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
                var isUp = _indicatorsServices.IsBackgroundUpAdvance(indicatorStock, date, period);
                //var isUp = _indicatorsServices.IsBackgroundUp(indicatorStock, date, period);
                
                var targetDay = indicatorStock.FirstOrDefault(d => d.Date.Date == date.Date);
                if(targetDay != null) 
                    targetDay.IsBackgroundUp = isUp;

                return targetDay ?? new StockDataIndicators();
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<List<StockDataIndicators>> FindHammers(string symbol, string zoom, string from, string to)
        {
            try
            {
                var stockData = await GetStockData(symbol, zoom, from, to);
                //TODO check volume
                var hammerCandles = stockData.Where(candle => 
                    _indicatorsServices.IsHammer(candle) &&
                    _indicatorsServices.CalculateAverageVolume(stockData, candle.Date, 40) < candle.Volume)
                    .ToList();
                return hammerCandles;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }   
        
        public async Task<List<StockDataIndicators>> FindBullishEngulfing(string symbol, string zoom, string from, string to)
        {
            try
            {
                var stockData = await GetStockData(symbol, zoom, from, to);

                var bullishEngulfingCandles = stockData
                    .Where((candle, index) => 
                    index > 0 &&
                    _indicatorsServices.IsBullishEngulfing(stockData[index - 1], stockData[index])
                    && _indicatorsServices.CalculateAverageVolume(stockData, stockData[index].Date, 5) < stockData[index].Volume
                    ).ToList();

                bullishEngulfingCandles.ForEach(candle => candle.PatternType = PatternTypeEnum.BullishEngulfing);

                return bullishEngulfingCandles;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<List<StockDataIndicators>> FindMornigStar(string symbol, string zoom, string from, string to)
        {
            try
            {
                var stockData = await GetStockData(symbol, zoom, from, to);

                var mornigStarCandles = stockData
                    .Where((candle, index) =>
                    index > 0 &&
                    index < stockData.Count - 1 &&
                    _indicatorsServices.IsMorningStarToday(stockData[index - 1], stockData[index], stockData[index + 1]))
                    .ToList();

                return mornigStarCandles;

            }    
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

    
    }
}
