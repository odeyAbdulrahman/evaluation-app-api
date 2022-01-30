using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OA.Api;
using OA.Api.Controllers;
using OA.Api.Filters;
using OA.Api.UnitOfWork;
using OA.Base.Helpers;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ConsumerFilter]
    //[Authorize(Roles = "Admin, GeneralDirector, Editor")]
    [Authorize]
    public class UserRoleController : BaseController
    {
        public UserRoleController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings) : base(unitOfWork, appSettings)
        {
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetAsync(string userId)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(userId);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            return Ok(await UnitOfWork.UserRoleService.GetAsync(User));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] UserMangeRolesViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(model.UserId);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            FeedBack Feed = await UnitOfWork.UserRoleService.PostAsync(User, model.Roles.Select(x => x.ToString()).ToList().AsEnumerable());
            return Ok(Response(Feed));
        }
        [HttpDelete("{userId}/{role}")]
        public async Task<ActionResult> DelAsync(string userId, string role)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(userId);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            FeedBack Feed = await UnitOfWork.UserRoleService.DelAsync(User, role);
            return Ok(Response(Feed));
        }
    }
}
