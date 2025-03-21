using AF_mobile_web_api_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.Services.Interfaces
{
    public interface IQuotesServices
    {
        Task<List<StockDataIndicators>> GetStockData(string symbol, string zoom, string from = "", string to = "");
        Task<List<StockDataIndicators>> GetStockEMA(string symbol, string zoom, int period);
        Task<StockDataIndicators> CheckBackground(string symbol, string zoom, string dateString);
        Task<List<StockDataIndicators>> FindHammers(string symbol, string zoom, string from, string to);
        Task<List<StockDataIndicators>> FindBullishEngulfing(string symbol, string zoom, string from, string to);
        Task<List<StockDataIndicators>> FindMornigStar(string symbol, string zoom, string from, string to);
    }
}
