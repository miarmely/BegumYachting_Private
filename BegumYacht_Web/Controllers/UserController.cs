using Microsoft.AspNetCore.Mvc;


namespace BegumYacht_Web.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Display()
		{
			return View();
		}
	}
}
