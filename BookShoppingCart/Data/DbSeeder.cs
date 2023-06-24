﻿using BookShoppingCart.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookShoppingCart.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr=service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            // adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // create admin User
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            var isUserExists=await userMgr.FindByEmailAsync(admin.Email);
            
            if(isUserExists == null)
            {
                await userMgr.CreateAsync(admin, "Mypassword123@");
                await userMgr.AddToRoleAsync(admin,Roles.Admin.ToString());
            }
        }
    }
}
