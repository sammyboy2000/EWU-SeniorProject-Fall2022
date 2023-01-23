using Microsoft.AspNetCore.Identity;
using Tutor.Api.Models;

namespace Tutor.Api.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            await SeedRolesAsync(roleManager);

            // Seed Super User
            await SeedSuperUserAsync(userManager);

            // Seed Admin User
            await SeedAdminUserAsync(userManager);

            // Seed Tutor User
            await SeedTutorUserAsync(userManager);

            // Seed Student User
            await SeedStudentUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Tutor))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Tutor));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Student))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Student));
            }
        }

        private static async Task SeedSuperUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Super User
            if (await userManager.FindByNameAsync("sshaw16@ewu.edu") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "sshaw16@ewu.edu",
                    Email = "sshaw16@ewu.edu",
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    await userManager.AddToRoleAsync(user, Roles.Tutor);
                    await userManager.AddToRoleAsync(user, Roles.Student);
                }
            }
        }
        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Super User
            if (await userManager.FindByNameAsync("admin@ewu.edu") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "admin@ewu.edu",
                    Email = "admin@ewu.edu",
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }
        }
        private static async Task SeedTutorUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Super User
            if (await userManager.FindByNameAsync("tutor@ewu.edu") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "tutor@ewu.edu",
                    Email = "tutor@ewu.edu",
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Tutor);
                }
            }
        }
        private static async Task SeedStudentUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Super User
            if (await userManager.FindByNameAsync("student@ewu.edu") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "student@ewu.edu",
                    Email = "student@ewu.edu",
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Student);
                }
            }
        }
    }
}