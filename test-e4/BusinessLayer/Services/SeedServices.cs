﻿using Microsoft.AspNetCore.Identity;
using test_e4.Data;
using test_e4.Models;

namespace test_e4.BusinessLayer.Services
{
    public class SeedServices
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedServices>>();

            try
            {
                // Ensure the database is ready
                logger.LogInformation("Ensuring database is created...");
                await context.Database.EnsureCreatedAsync();
            
                // Add roles
                logger.LogInformation("Seeding roles...");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");

                // Add admin user
                logger.LogInformation("Seeding admin user...");
                var admminEmail = "admin@gmail.com";
                if(await userManager.FindByEmailAsync(admminEmail) == null)
                {
                    var adminUser = new Users
                    {
                        FullName = "Admin User",
                        UserName = admminEmail,
                        NormalizedUserName = admminEmail.ToUpper(),
                        Email = admminEmail,
                        NormalizedEmail = admminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Assigning Admin role to the admin user.");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogInformation("Admin user already exists.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
