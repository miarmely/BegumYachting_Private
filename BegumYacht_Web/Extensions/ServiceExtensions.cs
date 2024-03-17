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

		public static void ConfigureAuthentication(this IServiceCollection services) =>
			services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(opt =>
				{
					opt.LoginPath = "/homepage/index";
					opt.LogoutPath = "/authentication/login";
				});

		public static void ConfigureMapControllerRoute(this WebApplication app)
		{
			#region user
			app.MapControllerRoute(
				name: "userCreate",
				pattern: "userCreate",
				defaults: new { controller = "User", action = "Create" });

			app.MapControllerRoute(
				name: "userDisplay",
				pattern: "userDisplay",
				defaults: new { controller = "User", action = "Display" });
			#endregion

			#region orders
			app.MapControllerRoute(
				name: "provisionOrder",
				pattern: "provisionOrder",
				defaults: new { controller = "Order", action = "Provision" });

			app.MapControllerRoute(
				name: "flowerOrder",
				pattern: "flowerOrder",
				defaults: new { controller = "Order", action = "Flower" });

			app.MapControllerRoute(
				name: "technicalAssistanceAndSparePartOrder",
				pattern: "technicalAssistanceAndSparePartOrder",
				defaults: new { controller = "Order", action = "TechnicalAssistanceAndSparePart" });
			#endregion

			#region demands
			app.MapControllerRoute(
				name: "fuelPurchaseDemand",
				pattern: "fuelPurchaseDemand",
				defaults: new { controller = "Demand", action = "FuelPurchase" });

			app.MapControllerRoute(
				name: "checkinAndCheckoutDemand",
				pattern: "checkinAndCheckoutDemand",
				defaults: new { controller = "Demand", action = "CheckinAndCheckout" });

			app.MapControllerRoute(
				name: "berthReservationDemand",
				pattern: "berthReservationDemand",
				defaults: new { controller = "Demand", action = "BerthReservation" });

			app.MapControllerRoute(
				name: "vipTransferDemand",
				pattern: "vipTransferDemand",
				defaults: new { controller = "Demand", action = "VipTransfer" });

			app.MapControllerRoute(
				name: "Demand",
				pattern: "Demand",
				defaults: new { controller = "Demand", action = "Display" });

			app.MapControllerRoute(
				name: "excursionDemand",
				pattern: "excursionDemand",
				defaults: new { controller = "Demand", action = "Excursion" });

			app.MapControllerRoute(
				name: "conciergeServiceDemand",
				pattern: "conciergeServiceDemand",
				defaults: new { controller = "Demand", action = "ConciergeService" });

			app.MapControllerRoute(
				name: "securityAndProtectionServiceDemand",
				pattern: "securityAndProtectionServiceDemand",
				defaults: new { controller = "Demand", action = "SecurityAndProtectionService" });
			#endregion

			#region others
			// when route is blank
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Authentication}/{action=Login}/{id?}");

			app.MapControllerRoute(
				name: "login",
				pattern: "login",
				defaults: new { controller = "Authentication", action = "Login" });

			app.MapControllerRoute(
				name: "logout",
				pattern: "logout",
				defaults: new { controller = "Authentication", action = "Logout" });

			app.MapControllerRoute(
				name: "homepage",
				pattern: "homepage",
				defaults: new { controller = "HomePage", action = "Index" });

			app.MapControllerRoute(
				name: "userProfile",
				pattern: "userProfile",
				defaults: new { controller = "Menubar", action = "Profile" });
			#endregion
		}
	}
}