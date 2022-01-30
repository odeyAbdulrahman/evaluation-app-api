using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OA.Api;
using OA.Api.Controllers;
using OA.Api.Filters;
using OA.Api.UnitOfWork;
using OA.Base.Enums;
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
    [Authorize]
    public class UserManageController : BaseController
    {
        public UserManageController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper) : base(unitOfWork, appSettings)
        {
            Mapper = mapper;
        }
        private readonly IMapper Mapper;

        private IEnumerable<string> Roles;
        private UserManageViewModel MapedUserInUserManageView;

        [HttpGet("{role}")]
        public async Task<ActionResult> GetAsync(string role)
        {
            return Ok(Mapper.Map<List<UserManageViewModel>>(await UnitOfWork.UserManageService.GetAsync(role)));
        }
        [HttpGet("{role}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(string role, int skip, int take)
        {
            return Ok(Mapper.Map<List<UserManageViewModel>>(await UnitOfWork.UserManageService.GetAsync(role, skip, take)));
        }
        [HttpGet("{searchBy}/{key}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(EnumUserSearchBy searchBy, string key, int skip, int take)
        {
            var List = await UnitOfWork.UserManageService.GetAsync(searchBy, key, take, skip);
            var Users = Mapper.Map<List<UserManageViewModel>>(List);
            return Ok(Users);
        }
        [HttpPost]
        [Route("GetUserInRoles")]
        public async Task<ActionResult> GetUserAsync([FromBody] string[] roles)
        {
            List<AspNetUser> Models = await UnitOfWork.UserManageService.GetAsync(roles);
            return Ok(Mapper.Map<List<UserManageViewModel>>(Models));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PostUserManageViewModel model)
        {
            AspNetUser MapedPostAccountInUser = Mapper.Map<AspNetUser>(model);
            MapedPostAccountInUser.DefaultRole = ((long)model.Role).ToString();
            MapedPostAccountInUser.DateCreated = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            (FeedBack, AspNetUser) Feed = await PostUserComposAsync(MapedPostAccountInUser);
            if (Feed.Item1 != FeedBack.AddedSuccess)
                return Ok(Response(Feed.Item1));
            await UnitOfWork.UserRoleService.PostAsync(Feed.Item2, Enum.GetName(model.Role));
            MapedUserInUserManageView = Mapper.Map<UserManageViewModel>(Feed.Item2);
            return Ok(Response(Feed.Item1, MapedUserInUserManageView));
        }
        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody] PutUserManageViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(model.Id);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            CurrentFile = User.UserImage;
            var MapedUpdateAccountInUser = Mapper.Map<PutUserManageViewModel, AspNetUser>(model, User);
            (FeedBack, AspNetUser) Feed = await PutUserComposAsync(MapedUpdateAccountInUser, CurrentFile);
            if (Feed.Item1 == FeedBack.EditedSuccess)
                MapedUserInUserManageView = Mapper.Map<UserManageViewModel>(Feed.Item2);
            return Ok(Response(Feed.Item1, MapedUserInUserManageView));
        }
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(model.UserId);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            var MapedChangePasswordInUser = Mapper.Map<ChangePasswordViewModel, AspNetUser>(model, User);
            string NewPassword = UnitOfWork.UserAuthService.PasswordHasher(User);
            MapedChangePasswordInUser.PasswordHash = NewPassword;
            FeedBack Feed = await UnitOfWork.UserAuthService.PutAsync(MapedChangePasswordInUser);
            if (Feed == FeedBack.EditedSuccess)
                MapedUserInUserManageView = Mapper.Map<UserManageViewModel>(MapedChangePasswordInUser);
            return Ok(Response(Feed, MapedUserInUserManageView));
        }
        [HttpPut]
        [Route("Archive")]
        public async Task<ActionResult> PutArchiveAsync([FromBody] ArchiveUserManageViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(model.Id);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            var MapedUpdateAccountInUser = Mapper.Map<ArchiveUserManageViewModel, AspNetUser>(model, User);
            FeedBack Feed = await UnitOfWork.UserAuthService.PutAsync(MapedUpdateAccountInUser);
            if (Feed != FeedBack.EditedSuccess)
                return Ok(Response(Feed));
            var Model = await UnitOfWork.UserAuthService.FindAsync(model.Id);
            var ViewModel = Mapper.Map<UserManageViewModel>(Model);
            return Ok(Response(Feed, ViewModel));
        }
        [HttpPut]
        [Route("AvailabilityStatus")]
        public async Task<ActionResult> PutAvailabilityStatusAsync([FromBody] AvailabilityStatusUserManageViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(model.Id);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            var MapedUpdateAccountInUser = Mapper.Map<AvailabilityStatusUserManageViewModel, AspNetUser>(model, User);
            FeedBack Feed = await UnitOfWork.UserAuthService.PutAsync(MapedUpdateAccountInUser);
            if (Feed != FeedBack.EditedSuccess)
                return Ok(Response(Feed));
            var Model = await UnitOfWork.UserAuthService.FindAsync(model.Id);
            var ViewModel = Mapper.Map<UserManageViewModel>(Model);
            return Ok(Response(Feed, ViewModel));
        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DelAsync(string userId)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(userId);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            Roles = await UnitOfWork.UserRoleService.GetAsync(User);
            await UnitOfWork.UserRoleService.DelAsync(User, Roles.Select(x => nameof(x)).ToList());
            FeedBack Feed = await UnitOfWork.UserAuthService.DelAsync(User);
            return Ok(Response(Feed));
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<(FeedBack, AspNetUser)> PostUserComposAsync(AspNetUser model)
        {
            if (UnitOfWork.UserAuthService.AnyByUserIdAndUserNameAndEmailAndPhoneNumber(model.Id, model.UserName, model.Email, model.PhoneNumber))
                return (FeedBack.IsExist, null);
            (FeedBack, AspNetUser) Feed = await UnitOfWork.UserManageService.PostAsync(model);
            return (Feed.Item1, Feed.Item2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CurrentFile"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<(FeedBack, AspNetUser)> PutUserComposAsync(AspNetUser model, string CurrentFile)
        {
            if (UnitOfWork.UserAuthService.AnyByUserIdAndUserNameAndEmailAndPhoneNumber(model.Id, model.UserName, model.Email, model.PhoneNumber))
                return (FeedBack.IsExist, null);
            FeedBack Feed = await UnitOfWork.UserAuthService.PutAsync(model);
            return (Feed, model);
        }
        #endregion
    }
}
