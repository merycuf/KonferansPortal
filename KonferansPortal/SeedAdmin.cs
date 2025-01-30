using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KonferansPortal
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<Uye> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            string adminRole = "Admin";
            string userRole = "User";
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            if (!await roleManager.RoleExistsAsync(userRole))
            {
                await roleManager.CreateAsync(new IdentityRole(userRole));
            }

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new Uye
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Name = "Admin",
                    Surname = "Admin"

                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }

            }

            if (await userManager.FindByEmailAsync("user1@example.com") == null)
            {
                // Seed 30 users
                for (int i = 1; i <= 30; i++)
                {
                    string email = $"user{i}@example.com";
                    var user = new Uye
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        Name = $"User{i}",
                        Surname = $"Surname{i}",
                        Discriminator = "Uye"
                    };

                    var userPassword = "User@"+ i+ "123";

                    var result = await userManager.CreateAsync(user, userPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }

            if (!context.Duyurular.Any())
            {
                
                List<Duyurular> duyuruSeed = new List<Duyurular>();
                for (int i = 1; i < 30 ; i++)
                {
                    context.Duyurular.Add(
                        new Duyurular
                        {
                            Title = "Announcement " + i,
                            Content = "This is the content of the announcement " + i,
                            Date = DateTime.Now
                        });
                }

                context.SaveChanges();
            }

        }
    }
}
