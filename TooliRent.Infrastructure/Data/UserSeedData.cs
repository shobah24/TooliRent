using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TooliRent.Domain.Models;

namespace TooliRent.Infrastructure.Data
{
    public class UserSeedData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            // Roller
            var roles = new[] { "Admin", "Member" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin user
            var adminEmail = "admin@toolirent.local";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Member user
            var memberEmail = "member@toolirent.local";
            if (await userManager.FindByEmailAsync(memberEmail) == null)
            {
                var member = new User
                {
                    UserName = "member",
                    Email = memberEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(member, "Member123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(member, "Member");
            }

        }
    }
}
