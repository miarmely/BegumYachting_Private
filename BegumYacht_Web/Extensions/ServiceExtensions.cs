using BegumYatch.Core.Configs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

		public static void ConfigureAuthentication(
			this IServiceCollection services,
			IConfiguration configuration)
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