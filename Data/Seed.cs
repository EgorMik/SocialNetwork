﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocialNetwork.Data
{
    public class Seed
    {
        //public static async Task SeedUsers(UserManager<AppUser> userManager,
        //   RoleManager<AppRole> roleManager)
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            //var roles = new List<AppRole>
            //{
            //    new AppRole{Name = "Member"},
            //    new AppRole{Name = "Admin"},
            //    new AppRole{Name = "Moderator"},
            //};

            //foreach (var user in users)
            //{
            //    await roleManager.CreateAsync(role);
               
            //}

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();

            //var admin = new AppUser
            //{
            //    UserName = "admin"
            //};

            //await userManager.CreateAsync(admin, "Pa$$w0rd");
            //await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
    }
}
