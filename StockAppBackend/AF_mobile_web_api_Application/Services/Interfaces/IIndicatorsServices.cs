using AF_mobile_web_api_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.Services.Interfaces
{
    public interface IIndicatorsServices
    {
        List<decimal> CalculateEma(List<decimal> prices, int period);
        bool IsBackgroundUp(List<StockDataIndicators> _data, DateTime date, int dateRange);
        bool IsBackgroundUpAdvance(List<StockDataIndicators> data, DateTime date, int dateRange);
        bool IsHammer(StockDataIndicators candle);
        decimal CalculateVolumeQuartile(List<StockDataIndicators> stockData, DateTime startDate, int quartile, int periods = 40);
        decimal CalculateAverageVolume(List<StockDataIndicators> stockData, DateTime startDate, int periods = 40);
    }
}
