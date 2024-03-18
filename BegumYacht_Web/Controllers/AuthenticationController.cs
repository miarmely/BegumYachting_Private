using BegumYatch.API.Filters.AdminPanel.Attributes;
using BegumYatch.Core.Configs;
using BegumYatch.Core.Enums.AdminPanel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BegumYacht_Web.Controllers
{
	public partial class AuthenticationController : Controller  // private
	{
		private readonly JwtSettingsConfig _jwtSettingsConfig;

		public AuthenticationController(
			IOptions<JwtSettingsConfig> jwtSettingsConfig) =>
				_jwtSettingsConfig = jwtSettingsConfig.Value;

		private async Task<bool> IsTokenInvalidAsync(string token)
		{
			#region validate token
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityKeyInBytes = Encoding.UTF8
				.GetBytes(_jwtSettingsConfig.SecretKey);

			var result = await tokenHandler.ValidateTokenAsync(token,
				new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = _jwtSettingsConfig.ValidIssuer,
					ValidateAudience = true,
					ValidAudience = _jwtSettingsConfig.ValidAudience1,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(securityKeyInBytes),
					ValidateLifetime = false
				});
			#endregion

			#region when token is invalid
			if (!result.IsValid)
				return true;
			#endregion

			return false;
		}
	}


	public partial class AuthenticationController  // public
	{
		public IActionResult Login() => View();

		public IActionResult Register() => View();

		public async Task<IActionResult> AfterLogin(
			[FromQuery(Name = "token")] string token)
		{
			#region when token is invalid (REDIRECT)
			if (!await SigninAsync(token))
				return Redirect("/login");
			#endregion

			return Redirect("/userDisplay");
		}


		[MiarWebAuthorize("Admin")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme);

			return Redirect("/login");
		}


		public async Task<bool> SigninAsync(
			[FromQuery(Name = "token")] string token)
		{
			#region check token wnnhether invalid
			if (await IsTokenInvalidAsync(token))
				return false;
			#endregion

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

			return true;
		}
	}
}