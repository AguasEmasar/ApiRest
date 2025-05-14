using LOGIN.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.Database
{
    public class ApplicationDbSeeder
    {
        public static async Task InitializeAsync
            (
            UserManager<UserEntity> userManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("ApplicationDbContextInitializer");

            try
            {
                await SeedRolesAsync(roleManager, logger);
                await SeedUsersAsync(userManager, logger);
                await SeedStatesAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            string[] roleNames = { "User", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Seeded {roleName} role.");
                    }
                    else
                    {
                        logger.LogError($"Failed to create {roleName} role.");
                    }
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<UserEntity> userManager, ILogger logger)
        {
            var adminEmail = "aguassantarosa6@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new UserEntity
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    CreatedDate = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Seeded Admin user.");
                }
                else
                {
                    logger.LogError("Failed to create Admin user.");
                }
            }
        }

        private static async Task SeedStatesAsync(ApplicationDbContext context, ILogger logger)
        {
            var states = new StateEntity[]
            {
                new StateEntity { Name = "no asignado" },
                new StateEntity { Name = "asignado" }
            };

            foreach (var state in states)
            {
                if (!await context.States.AnyAsync(s => s.Name == state.Name))
                {
                    await context.States.AddAsync(state);
                }
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seeded states.");
        }
    }
}
