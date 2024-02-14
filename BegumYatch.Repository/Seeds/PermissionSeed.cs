using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Permissions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {

            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");

            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });
                var basicRole = (await roleManager.FindByNameAsync("BasicRole"))!;
                await AddReadPermission(basicRole, roleManager);
            }
            if (!hasAdvancedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvancedRole" });
                var advancedRole = (await roleManager.FindByNameAsync("AdvancedRole"))!;
                await AddReadPermission(advancedRole, roleManager);
                await AddUpdateAndCreatePermission(advancedRole, roleManager);
            }
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });
                var adminRole = (await roleManager.FindByNameAsync("AdminRole"))!;
                await AddReadPermission(adminRole, roleManager);
                await AddUpdateAndCreatePermission(adminRole, roleManager);
                await AddDeletePermission(adminRole, roleManager);
            }

        }

        public static async Task AddReadPermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Demand.Read));
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Read));
        }

        public static async Task AddUpdateAndCreatePermission(AppRole role, RoleManager<AppRole> roleManager)
        {

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Demand.Create));
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Create));
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Demand.Update));
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Update));
        }
        public static async Task AddDeletePermission(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Demand.Delete));
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Delete));
        }
    }
}
