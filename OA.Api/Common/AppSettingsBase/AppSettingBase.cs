using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Data;
using OA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Api.Common.AppSettingsBase
{
    public static class AppSettingBase
    {
        public static async Task AppSetting(this IServiceProvider serviceProvider, AppDbContext db)
        {
            db.Database.EnsureCreated();
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AspNetUser>>();
            bool x = await RoleManager.RoleExistsAsync(nameof(EnumUserRole.Admin));
            #region roles
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole {
                    Id = ((long)EnumUserRole.Admin).ToString(),
                    Name = nameof(EnumUserRole.Admin)
                };
                await RoleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   
                var user = new AspNetUser { UserName = "Eval", FullName = "Eval app", FullNameAr = "Eval app", Email = "Eval@app.com", CountryCode = "+971", PhoneNumber = "56342321", DateCreated = DateTime.Now, Blance = 0 };
                string UserPWD = "Eval@sh2022";
                IdentityResult chkUser = await UserManager.CreateAsync(user, UserPWD);
                //
                
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, nameof(EnumUserRole.Admin));
                    var dept = new Department { Name = "Anonymous", NameAr = "Anonymous", CreatedDate = DateTime.Now, CreatedBy = user.Id };
                    db.Add(dept);
                    db.SaveChanges();
                }
            }
            //1
            x = await RoleManager.RoleExistsAsync(nameof(EnumUserRole.User));
            if (!x)
            {
                var role = new IdentityRole
                {
                    Id = ((long)EnumUserRole.User).ToString(),
                    Name = nameof(EnumUserRole.User)
                };
                await RoleManager.CreateAsync(role);
            }
            //4
            x = await RoleManager.RoleExistsAsync(nameof(EnumUserRole.Editor));
            if (!x)
            {
                var role = new IdentityRole
                {
                    Id = ((long)EnumUserRole.Editor).ToString(),
                    Name = nameof(EnumUserRole.Editor)
                };
                await RoleManager.CreateAsync(role);
            }
            //5
            x = await RoleManager.RoleExistsAsync(nameof(EnumUserRole.Customer));
            if (!x)
            {
                var role = new IdentityRole
                {
                    Id = ((long)EnumUserRole.Customer).ToString(),
                    Name = nameof(EnumUserRole.Customer)
                };
                await RoleManager.CreateAsync(role);
            }
            //8
            x = await RoleManager.RoleExistsAsync(nameof(EnumUserRole.GeneralDirector));
            if (!x)
            {
                var role = new IdentityRole
                {
                    Id = ((long)EnumUserRole.GeneralDirector).ToString(),
                    Name = nameof(EnumUserRole.GeneralDirector)
                };
                await RoleManager.CreateAsync(role);
            }
            //12
            x = await RoleManager.RoleExistsAsync(nameof(EnumUserRole.Employee));
            if (!x)
            {
                var role = new IdentityRole
                {
                    Id = ((long)EnumUserRole.Employee).ToString(),
                    Name = nameof(EnumUserRole.Employee)
                };
                await RoleManager.CreateAsync(role);
            }
            #endregion
        }
    }
}
