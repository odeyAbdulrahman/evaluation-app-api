using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OA.Base.Helpers;
using OA.Data.Models;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Service.Services
{
    class UserManage : IUserManage
    {
        static string Role { get; set; }
        static string[] Roles { get; set; }
        static string SearchValue { get; set; }

        private readonly UserManager<AspNetUser> UserManager;
        public UserManage(UserManager<AspNetUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<List<AspNetUser>> GetAsync(string role)
        {
            Role = role;
            return await UserManager.Users.OrderByDescending(x => x.DateCreated).Where(x => x.DefaultRole == Role && x.Archive != true).ToListAsync();
        }
        public async Task<List<AspNetUser>> GetAsync(string[] roles)
        {
            Roles = roles;
            return await UserManager.Users.OrderByDescending(x => x.DateCreated).Where(x =>  x.Archive != true && Roles.Contains(x.DefaultRole)).ToListAsync();
        }
        public async Task<List<AspNetUser>> GetAsync(string role, int skip, int take)
        {
            Role = role;
            return await UserManager.Users.OrderByDescending(x => x.DateCreated).
                   Where(x => x.DefaultRole == Role).Skip(skip).Take(take).ToListAsync();
        }
        public async Task<List<AspNetUser>> GetAsync(string[] roles, int skip, int take)
        {
            Roles = roles;
            return await UserManager.Users.OrderByDescending(x => x.DateCreated).Where(x => x.Archive != true && Roles.Contains(x.DefaultRole)).Skip(skip).Take(take).ToListAsync();
        }
        public async Task<List<AspNetUser>> GetAsync(EnumUserSearchBy searchBy, string searchValue, int skip, int take)
        {
            SearchValue = searchValue;
            if (searchBy == EnumUserSearchBy.ByName)
                return await UserManager.Users.OrderByDescending(x => x.DateCreated).Where(x => x.Archive == true).
                             Where(x => x.UserName.Contains(SearchValue)).Skip(skip).Take(take).ToListAsync();
            if (searchBy == EnumUserSearchBy.ByEmail)
                return await UserManager.Users.OrderByDescending(x => x.DateCreated).Where(x => x.Archive == true).
                             Where(x => x.Email.Contains(SearchValue)).Skip(skip).Take(take).ToListAsync();
            if (searchBy == EnumUserSearchBy.ByPhone)
                return await UserManager.Users.OrderByDescending(x => x.DateCreated).Where(x => x.Archive == true).
                             Where(x => x.Email.Contains(SearchValue)).Skip(skip).Take(take).ToListAsync();
            if (searchBy == EnumUserSearchBy.ByPhone)
                return await UserManager.Users.OrderByDescending(x => x.DateCreated).
                             Where(x => x.Archive == true).Skip(skip).Take(take).ToListAsync();
            return null;
        }
        public async Task<(FeedBack, AspNetUser)> PostAsync(AspNetUser model)
        {
            var result = await UserManager.CreateAsync(model, model.PasswordHash);
            return result.Succeeded ? (FeedBack.AddedSuccess, model) : (FeedBack.AddedFail, null);
        }
    }
}
