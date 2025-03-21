using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.DTO
{
    class StockWatch
    {
        public List<Quote> Quotes { get; set; }
        public List<Volume> Volume { get; set; }
    }

    public class Quote
    {
        public DateTime Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }

    public class Volume
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
        public string Color { get; set; }
    }
    
}
