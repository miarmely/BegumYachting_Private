using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Repository;
using Microsoft.AspNetCore.Identity;


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
    }
}
