using BegumYatch.Core.Configs;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BegumYacht_Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureConfigModels(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettingsConfig>(configuration
                .GetSection(nameof(JwtSettingsConfig)));
        }

        public static void ConfigureCookie(this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/homepage/index";
                    opt.LogoutPath = "/authentication/login";
                });
        }
    }
}
