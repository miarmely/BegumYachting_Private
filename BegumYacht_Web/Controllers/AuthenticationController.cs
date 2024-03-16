using BegumYatch.API.Filters.AdminPanel.Attributes;
using BegumYatch.Core.Configs;
using BegumYatch.Core.Enums.AdminPanel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly string[] _validRoles = new string[]
        {
           "Admin"
        };  // for login panel

        public AuthenticationController(
            IOptions<JwtSettingsConfig> jwtSettingsConfig) =>
                _jwtSettingsConfig = jwtSettingsConfig.Value;
    }


    public partial class AuthenticationController  // actions
    {
        public IActionResult Login() => View();
        
        public IActionResult Register() => View();
        
        public async Task<IActionResult> AfterLogin(
            [FromQuery(Name = "token")] string token)
        {
            #region sign in
            var jwtTokenClaims = new JwtSecurityToken(token).Claims;

            var claimsIdentity = new ClaimsIdentity(
                jwtTokenClaims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);
            #endregion

            return NoContent();
        }

        public async Task<IActionResult> OpenHomepage()
        {
            return RedirectToAction("Index", "HomePage");
        }


        //[MiarWebAuthorize("Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}