using System;
using Leave_Management.Data;
using Microsoft.AspNetCore.Identity;

namespace Leave_Management
{
    public static class SeedData
    {
        public static void Seed(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeeedUsers(userManager);
        }

        public static void SeeedUsers(UserManager<Employee> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var user = new Employee
                {
                    UserName = "admin",
                    Email = "admin@localhost.com",
                    Lastname = "-",
                    Firstname = "مدیر",
                    PhoneNumber = "سیستم",
                    DateCreated = DateTime.Now,
                    DateJoined = DateTime.Now,
                    DateOfBirth = DateTime.Now,
                    EmailConfirmed = true,
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN",
                    TwoFactorEnabled = false,
                    
                };

                var result = userManager.CreateAsync(user, "P@ssw0rd1").Result;
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
                var result = roleManager.CreateAsync(role).Result;
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