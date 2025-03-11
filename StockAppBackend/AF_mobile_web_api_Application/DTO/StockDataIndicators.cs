using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF_mobile_web_api_Application.DTO
{
    public class StockDataIndicators: TwistData
    {
        public decimal EMA { get; set; }
        public bool IsBackgroundUp { get; set; }
    }
}
