using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StockAppBackaned.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		public CompanyController() { }


		[AllowAnonymous]
		[HttpGet("GetNumber")]
		public double GetNumber()
		{
			return 25;
		}
	}
}
