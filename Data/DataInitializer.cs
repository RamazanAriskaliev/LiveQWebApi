using LiveQ.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Data
{
    public static class DataInitializer
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            await SeedRoles(serviceProvider);
            await SeedUsers(serviceProvider);
        }


        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            // Create test users
            var user = userManager.FindByNameAsync("admin");
            if (user == null)
            {
                var newUser = new AppUser()
                {
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "ramazan.ariskaliev@gmail.com",
                    PhoneNumber = "998913959334"
                };
                await userManager.CreateAsync(newUser, "Password1!");
                
                await userManager.AddToRoleAsync(newUser, "Admin");
            }
        }

        public enum Roles
        {
            Administrator,
            Creator,
            Subscriber
        }
        /*
        public static void Initialize(IServiceProvider provider)
        {
            var _context = provider.GetRequiredService<ApplicationDbContext>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!_context.Users.Any())
            {

                // Add missing roles
                var role = roleManager.FindByNameAsync("Admin");
                if (role == null)
                {
                    roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create test users
                var user = userManager.FindByNameAsync("admin");
                if (user == null)
                {
                    var newUser = new AppUser()
                    {
                        UserName = "admin",
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "xxx@xxx.net",
                        PhoneNumber = "5551234567",
                        MustChangePassword = false
                    };
                    userManager.Create(newUser, "Password1");
                    userManager.SetLockoutEnabled(newUser.Id, false);
                    userManager.AddToRole(newUser.Id, "Admin");
                }
            }*/
    }
}
