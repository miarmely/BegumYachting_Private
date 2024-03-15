using BegumYatch.API.Filters.AdminPanel.Attributes;
using BegumYatch.Core.Configs;
using BegumYatch.Core.Enums.AdminPanel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
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

        private async Task<bool> isUserRoleValidAsync(IEnumerable<Claim> claims)
        {
            var roleName = await GetClaimValueAsync(
                claims,
                MiarClaimTypes.Role.ToString());

            return _validRoles.Any(r => r.Equals(roleName));
        }

        private async Task<string> GetClaimValueAsync(
            IEnumerable<Claim> claims,
            string claimType) =>
                claims
                    .FirstOrDefault(c => c.Type.Equals(claimType))
                    .Value;
    }

    public partial class AuthenticationController  // actions
    {
        public IActionResult Login() => View();
        public IActionResult ForgotPassword() => View();
        public IActionResult Register() => View();
        
        public async Task<IActionResult> AfterLogin(
            [FromQuery(Name = "token")] string token)
        {
            #region check user role whether valid
            var jwtTokenClaims = new JwtSecurityToken(token).Claims;

            if (!await isUserRoleValidAsync(jwtTokenClaims))
                return NotFound();
            #endregion

            #region sign in
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


        [MiarWebAuthorize("Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}