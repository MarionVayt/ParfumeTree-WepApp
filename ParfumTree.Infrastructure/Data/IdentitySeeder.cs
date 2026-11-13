using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ParfumTree.Domain.Common;
using ParfumTree.Domain.Entities;

namespace ParfumTree.Infrastructure.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, string? adminEmail, string? adminPassword)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var role in new[] { AppRoles.Admin, AppRoles.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var created = await roleManager.CreateAsync(new IdentityRole<int>(role));
                if (!created.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Не вдалося створити роль {role}: {string.Join("; ", created.Errors.Select(e => e.Description))}");
                }
            }
        }

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "ParfumTree",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Не вдалося створити адміна: {string.Join("; ", result.Errors.Select(e => e.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
        {
            await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }
    }
}