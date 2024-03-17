using Autofac.Core;
using BegumYatch.Core.Configs;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Services;
using BegumYatch.Repository;
using BegumYatch.Service.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace BegumYatch.API.Extensions
{
	public static partial class ProgramcsExtensions
	{
		public static void AddIdentityExt(this IServiceCollection services)
		{
			services.Configure<DataProtectionTokenProviderOptions>(opt =>
			{
				opt.TokenLifespan = TimeSpan.FromHours(2);
			});

			services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_!.?@";
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireDigit = false;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
				options.Lockout.MaxFailedAccessAttempts = 3;
			})
			  .AddDefaultTokenProviders()
			  .AddEntityFrameworkStores<AppDbContext>();
		}
	}

	public static partial class ProgramcsExtensions  // By MERT
	{
		public static void ConfigureCors(this IServiceCollection services) =>
			services.AddCors(setup =>
			{
				setup.AddDefaultPolicy(config =>
				{
					config.WithOrigins(
							"http://localhost:3408",
							"https://localhost:3408")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.WithExposedHeaders(
							"Demand-FuelPurchase",
							"Demand-CheckinAndCheckout",
							"Demand-BerthReservation",
							"Demand-VipTransfer",
							"Demand-Excursion",
							"Demand-ConciergeService",
							"Demand-SecurityAndProtectionService",
							"Order-Provision",
							"Order-Flower",
							"Order-TechnicalAssistanceAndSparePart");
				});
			});

		public static void ConfigureJwt(
			this IServiceCollection services,
			IConfiguration configuration) =>
			services
				.AddAuthentication(opt =>
				{
					opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(opt =>
				{
					var section = configuration.GetSection(nameof(JwtSettingsConfig));

					opt.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = section["ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = section["ValidAudience1"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding
							.UTF8
							.GetBytes(section["SecretKey"])),
						ValidateLifetime = false  // close expires
					};
				});
		

		public static void ConfigureConfigModels(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.Configure<JwtSettingsConfig>(configuration
				.GetSection(nameof(JwtSettingsConfig)));
		}

		public static void ConfigureServices(
			this IServiceCollection services)
		{
			services.AddScoped<
				IBaseDemandAndOrderService,
				BaseDemandAndOrderService>();

			services.AddScoped<IRoleService, RoleService>();
		}
	}
}
