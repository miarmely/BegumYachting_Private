using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BegumYacht_Web.Controllers
{
	public class AuthenticationController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		public async Task<IActionResult> AfterLogin(
			[FromQuery(Name = "AccountId")] string accountId)
		{
			#region sign in
			var claims = new List<Claim>
			{
				new (ClaimTypes.SerialNumber, accountId),
			};
			var claimsIdentity = new ClaimsIdentity(claims);
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				claimsPrincipal);
			#endregion

			return RedirectToAction("Index", "HomePage");
		}
	}
}
