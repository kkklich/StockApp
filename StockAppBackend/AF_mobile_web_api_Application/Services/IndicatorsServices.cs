using AF_mobile_web_api_Application.DTO;
using AF_mobile_web_api_Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
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

            var accumulation = previousDays.Count(d => d.Close > d.Open && d.Volume > previousDays.Average(p => p.Volume));
            var distribution = previousDays.Count(d => d.Close < d.Open && d.Volume > previousDays.Average(p => p.Volume));

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
                    if (current.Close > current.Open && current.Volume > previous.Volume * 2 && current.High > previous.High)
                    {
                        accumulationCount++;
                    }
                    // Selling Climax
                    if (current.Close < current.Open && current.Volume > previous.Volume * 2 && current.Low < previous.Low)
                    {
                        distributionCount++;
                    }
                    // No Demand
                    if (current.Close > current.Open && current.Volume < previous.Volume * 0.5 && current.High <= previous.High)
                    {
                        distributionCount++;
                    }
                    // No Supply
                    if (current.Close < current.Open && current.Volume < previous.Volume * 0.5 && current.Low >= previous.Low)
                    {
                        accumulationCount++;
                    }
                    // Upthrust
                    if (current.Close < current.Open && current.Volume > previous.Volume && current.High > previous.High)
                    {
                        distributionCount++;
                    }
                    // Test for Supply
                    if (current.Close > current.Open && current.Volume < previous.Volume && current.Low < previous.Low)
                    {
                        accumulationCount++;
                    }
                }
            }

            return accumulationCount > distributionCount;
        }

        public bool IsHammer(StockDataIndicators candle)
        {
            decimal bodyLength = Math.Abs(candle.Close - candle.Open);
            decimal lowerShadow = Math.Min(candle.Open, candle.Close) - candle.Low;
            decimal upperShadow = candle.High - Math.Max(candle.Open, candle.Close);

            return lowerShadow > 2 * bodyLength && upperShadow < bodyLength;
        }

        public decimal CalculateAverageVolume(List<StockDataIndicators> stockData, DateTime startDate, int periods = 40)
        {
            var selectedPeriodData = stockData
                .Where(candle => candle.Date <= startDate)
                .OrderByDescending(candle => candle.Date)
                .Take(periods)
                .ToList();
                       
            long totalVolume = selectedPeriodData.Sum(candle => candle.Volume);
            return totalVolume / (decimal)periods;
        }

        public decimal CalculateVolumeQuartile(List<StockDataIndicators> stockData, DateTime startDate, int quartile, int periods = 40)
        {
            if (quartile < 1 || quartile > 3)
            {
                throw new ArgumentException("Quartile must be between 1 and 3.");
            }

            var selectedPeriodData = stockData
                .Where(candle => candle.Date <= startDate)
                .OrderByDescending(candle => candle.Date)
                .Take(periods)
                .Select(candle => candle.Volume)
                .OrderBy(volume => volume)
                .ToList();

            if (selectedPeriodData.Count < periods)
            {
                throw new ArgumentException("Not enough data to calculate the volume quartiles.");
            }

            int index = (int)Math.Ceiling(quartile * (selectedPeriodData.Count / 4.0)) - 1;
            return selectedPeriodData[index];
        }
              

        public bool IsBullishEngulfing(StockDataIndicators previousDay, StockDataIndicators currentDay)
        {
            // Check if the current day is bullish (close > open)
            bool isCurrentDayBullish = currentDay.Close > currentDay.Open;

            // Check if the previous day is bearish (close < open)
            bool isPreviousDayBearish = previousDay.Close < previousDay.Open;

            // Check if the current day's body engulfs the previous day's body
            bool isEngulfing = currentDay.Open < previousDay.Close && currentDay.Close > previousDay.Open;

            // Check if the volume is higher than the previous day's volume
            bool isVolumeHigher = currentDay.Volume > previousDay.Volume;

            // Calculate the body of the current day (close - open)
            decimal currentDayBody = currentDay.Close - currentDay.Open;

            // Calculate the upper shadow of the current day (high - close)
            decimal upperShadow = currentDay.High - currentDay.Close;

            // Check if the upper shadow is less than or equal to 25% of the body
            bool isUpperShadowValid = upperShadow <= 0.25m * currentDayBody;

            return isCurrentDayBullish && isPreviousDayBearish && isEngulfing && isVolumeHigher && isUpperShadowValid;
        }

        public bool IsMorningStarToday(StockDataIndicators candle1, StockDataIndicators candle2, StockDataIndicators candle3)
        {
            // 1) First candle: bearish with a relatively large body.
            bool firstIsBearish = (candle1.Close < candle1.Open);

            // 2) Second candle: small body (could be a doji or spinning top).
            var secondBodySize = (double)Math.Abs(candle2.Close - candle2.Open);
            var firstBodySize = (double)Math.Abs(candle1.Close - candle1.Open);
            var thirdBodySize = (double)Math.Abs(candle3.Close - candle3.Open);
            double avgBodySize = (firstBodySize + secondBodySize + thirdBodySize) / 3;
            bool secondIsSmallBody = (secondBodySize < (avgBodySize * 0.5));

            // 3) Third candle: bullish, closing above the midpoint of the first candle's body.
            bool thirdIsBullish = (candle3.Close > candle3.Open);
            var firstBodyMidpoint = candle1.Open + (candle1.Close - candle1.Open) / 2;
            bool closesAboveMidpoint = (candle3.Close > firstBodyMidpoint);

            return firstIsBearish && secondIsSmallBody && thirdIsBullish && closesAboveMidpoint;
        }

    }
}
