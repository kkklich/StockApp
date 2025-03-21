using AF_mobile_web_api_Application.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.DTO
{
    public class StockDataIndicators
    {
        public decimal High { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public decimal Low { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal EMA { get; set; }
        public bool IsBackgroundUp { get; set; }
        public PatternTypeEnum PatternType { get; set; }
    }
}
