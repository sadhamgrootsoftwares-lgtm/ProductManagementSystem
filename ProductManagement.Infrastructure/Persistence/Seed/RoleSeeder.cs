using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Persistence.Seed;

public static class RoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            serviceProvider.GetRequiredService<UserManager<AppUser>>();

        string[] roles = { "Admin", "Manager", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }

        // Default Admin
        var adminEmail = "Admin@gmail.com";

        var adminUser = await userManager
            .FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var user = new AppUser
            {
                FirstName = "System",
                LastName = "Admin",
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            try
            {
                var result = await userManager
              .CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
