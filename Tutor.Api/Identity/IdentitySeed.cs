using Microsoft.AspNetCore.Identity;
using Tutor.Api.Models;

namespace Tutor.Api.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, tutor_dbContext context)
        {
            // Seed Roles
            await SeedRolesAsync(roleManager);

            // Seed Super User
            await SeedSuperUserAsync(userManager,  context);

            // Seed Admin User
            await SeedAdminUsersAsync(userManager, context);

            // Seed Tutor User
            await SeedTutorUserAsync(userManager, context);

            // Seed Student User
            await SeedStudentUserAsync(userManager, context);
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

        private static async Task SeedSuperUserAsync(UserManager<AppUser> userManager, tutor_dbContext context)
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
                    ApiUser appUser = new();
                    appUser.ExternalId = user.UserName;
                    appUser.IsAdmin = true;
                    appUser.IsTutor = true;
                    appUser.IsStudent = true;
                    context.ApiUsers.Add(appUser);
                    
                    await userManager.AddToRoleAsync(user, Roles.Admin);

                    Admin a = new Admin();
                    a.UserId = appUser.UserId;
                    a.FirstName = "Samuel";
                    a.LastName = "Shaw";
                    context.Admins.Add(a);

                    await userManager.AddToRoleAsync(user, Roles.Tutor);

                    Models.Tutor t = new();
                    t.UserId = appUser.UserId;
                    t.FirstName = "Samuel";
                    t.LastName = "Shaw";
                    context.Tutors.Add(t);

                    await userManager.AddToRoleAsync(user, Roles.Student);

                    Student s = new();
                    s.UserId = appUser.UserId;
                    s.FirstName = "Samuel";
                    s.LastName = "Shaw";
                    s.Email = appUser.ExternalId;
                    context.Students.Add(s);

                    context.SaveChanges();
                }
            }
        }
        private static async Task SeedAdminUsersAsync(UserManager<AppUser> userManager, tutor_dbContext context)
        {
            // Seed Admin Users
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

            if (await userManager.FindByNameAsync("ssteiner@ewu.edu") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "ssteiner@ewu.edu",
                    Email = "ssteiner@ewu.edu",
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }
        }
        private static async Task SeedTutorUserAsync(UserManager<AppUser> userManager, tutor_dbContext context)
        {
            // Seed Tutor User
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
        private static async Task SeedStudentUserAsync(UserManager<AppUser> userManager, tutor_dbContext context)
        {
            // Seed Student User
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