using BegumYatch.Core.Configs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BegumYacht_Web.Controllers
{
    public partial class AuthenticationController : Controller  // main
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

    public partial class AuthenticationController  // actions
    {
        public IActionResult Login() => View();
        public IActionResult ForgotPassword() => View();
        public IActionResult Register() => View();

        public async Task<IActionResult> AfterLogin(
            [FromQuery(Name = "token")] string token)
        {
            // i took token for block the direct access to "afterLogin" with url

            #region when token invalid (REDIRECT)
            if (await IsTokenInvalidAsync(token))
                return RedirectToAction(
                    "Login",
                    "Authentication");
            #endregion

            #region set claimsIdentity
            var jwtTokenClaims = new JwtSecurityToken(token)
                  .Claims;

            var claimsIdentity = new ClaimsIdentity(
                jwtTokenClaims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            #region sign in
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            #endregion

            #region save claim infos to ViewBag
            ViewBag.ClaimInfos = new Dictionary<string, string>();

            foreach (var claim in jwtTokenClaims)
            {
                ViewBag.ClaimInfos[claim.Type] = claim.Value;
            }
            #endregion

            return RedirectToAction("Display", "User");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
