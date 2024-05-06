using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Infrastructure.Dto
{
	public class StooqModel
	{
		public DateTime Date { get; set; }
		public double Open { get; set; }
		public double High { get; set; }
		public double Low { get; set; }
		public double Close { get; set; }
		public double Volumen { get; set; }
	}
}
