using BegumYatch.API.Filters.AdminPanel.Attributes;
using BegumYatch.Core.Enums.AdminPanel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace BegumYacht_Web.Controllers
{
	public class AuthenticationController : Controller
	{
		public IActionResult Login() => View();

		public IActionResult Register() => View();

		public async Task<IActionResult> AfterLogin(
			[FromQuery(Name = "token")] string token)
		{
			#region sign in
			var jwtToken = new JwtSecurityToken(token);

			var claimIdentities = new List<ClaimsIdentity>
			{
				new ClaimsIdentity(
					jwtToken.Claims, 
					CookieAuthenticationDefaults.AuthenticationScheme),

				// add token to claims
				new ClaimsIdentity(
					new List<Claim> { new Claim(MiarClaimTypes.Token, token) },
					CookieAuthenticationDefaults.AuthenticationScheme),
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimIdentities),
				new AuthenticationProperties()
				{
					AllowRefresh = true,
					IsPersistent = true,
				});
			#endregion

			return Redirect("/homepage");
		}


		[MiarWebAuthorize("Admin")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme);

			return Redirect("/login");
		}
	}
}