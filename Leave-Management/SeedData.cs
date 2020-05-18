using Microsoft.AspNetCore.Identity;

namespace Leave_Management
{
    public static class SeedData
    {
        public static void Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeeedUsers(userManager);
        }

        public static void SeeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@localhost"
                };
                var result = userManager.CreateAsync(user, "@Admin001").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                };
                var result=roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}