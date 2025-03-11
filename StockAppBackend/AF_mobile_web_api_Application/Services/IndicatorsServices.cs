using AF_mobile_web_api_Application.DTO;
using AF_mobile_web_api_Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.Services
{
    public class IndicatorsServices : IIndicatorsServices
    {
        public IndicatorsServices() { }

        public List<decimal> CalculateEma(List<decimal> prices, int period)
        {
            if (prices == null || prices.Count < period)
            {
                throw new ArgumentException("Price list is either null or does not contain enough elements.");
            }

            var emaList = new List<decimal>();
            decimal smoothingFactor = 2.0m / (period + 1);
            decimal previousEma = prices.Take(period).Average(); // Initial EMA is the SMA of the first 'period' prices

            emaList.Add(Math.Round(previousEma, 2));

            for (int i = period; i < prices.Count; i++)
            {
                decimal currentEma = (prices[i] - previousEma) * smoothingFactor + previousEma;
                currentEma = Math.Round(currentEma, 2);
                emaList.Add(currentEma);
                previousEma = currentEma;
            }

            return emaList;
        }


        public bool IsBackgroundUp(List<StockDataIndicators> data, DateTime date, int dateRange)
        {
            var targetDay = data.FirstOrDefault(d => d.Date.Date == date.Date);
            if (targetDay == null)
            {
                throw new ArgumentException("No data available for the specified date.");
            }

            var previousDays = data.Where(d => d.Date < date).OrderByDescending(d => d.Date).Take(dateRange).ToList();
            if (previousDays.Count < dateRange)
            {
                throw new ArgumentException("Insufficient data to analyze background.");
            }

            // Implement VSA logic to analyze the background
            // This is a simplified example and should be expanded with real VSA analysis

            var accumulation = previousDays.Count(d => d.Last > d.Open && d.Volume > previousDays.Average(p => p.Volume));
            var distribution = previousDays.Count(d => d.Last < d.Open && d.Volume > previousDays.Average(p => p.Volume));

            return accumulation > distribution;
        }

        public bool IsBackgroundUpAdvance(List<StockDataIndicators> data, DateTime date, int dateRange)
        {
            var targetDay = data.FirstOrDefault(d => d.Date.Date == date.Date);
            if (targetDay == null)
            {
                throw new ArgumentException("No data available for the specified date.");
            }

            var previousDays = data.Where(d => d.Date < date).OrderByDescending(d => d.Date).Take(dateRange).ToList();
            if (previousDays.Count < dateRange)
            {
                throw new ArgumentException("Insufficient data to analyze background.");
            }

            // Implement VSA logic to analyze the background
            var accumulationCount = 0;
            var distributionCount = 0;

            for (int i = 0; i < previousDays.Count; i++)
            {
                var current = previousDays[i];
                var previous = i < previousDays.Count - 1 ? previousDays[i + 1] : null;
                var next = i > 0 ? previousDays[i - 1] : null;

                if (previous != null)
                {
                    // Buying Climax
                    if (current.Last > current.Open && current.Volume > previous.Volume * 2 && current.High > previous.High)
                    {
                        accumulationCount++;
                    }
                    // Selling Climax
                    if (current.Last < current.Open && current.Volume > previous.Volume * 2 && current.Low < previous.Low)
                    {
                        distributionCount++;
                    }
                    // No Demand
                    if (current.Last > current.Open && current.Volume < previous.Volume * 0.5 && current.High <= previous.High)
                    {
                        distributionCount++;
                    }
                    // No Supply
                    if (current.Last < current.Open && current.Volume < previous.Volume * 0.5 && current.Low >= previous.Low)
                    {
                        accumulationCount++;
                    }
                    // Upthrust
                    if (current.Last < current.Open && current.Volume > previous.Volume && current.High > previous.High)
                    {
                        distributionCount++;
                    }
                    // Test for Supply
                    if (current.Last > current.Open && current.Volume < previous.Volume && current.Low < previous.Low)
                    {
                        accumulationCount++;
                    }
                }
            }

            return accumulationCount > distributionCount;
        }
    }
}
