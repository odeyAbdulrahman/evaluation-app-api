using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using OA.Api.Filters;
using OA.Api.UnitOfWork;
using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Data.Models;
using OA.Dtos;
using OA.Dtos.ServiceViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ConsumerFilter]
    [Authorize]
    public class UserAuthController : BaseController
    {
        public UserAuthController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper, IFeatureManager featureManager) : base(unitOfWork, appSettings)
        {
            FeatureManager = featureManager;
            Mapper = mapper;
        }
        private readonly IMapper Mapper;
        private readonly IFeatureManager FeatureManager;

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            if (string.IsNullOrEmpty(CurrentUser()))
                return Ok(Response(FeedBack.Unauthorized));

            AspNetUser Model = await UnitOfWork.UserAuthService.FindAsync(CurrentUser());
            var Users = Mapper.Map<UserViewModel>(Model);
            return Ok(Users);
        }
        [HttpGet("{UserId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Ok(Response(FeedBack.Unauthorized));
            var getRow = await UnitOfWork.UserAuthService.FindAsync(userId);
            if (getRow is null)
                return Ok(Response(FeedBack.NotFound));
            var Users = Mapper.Map<UserManageViewModel>(getRow);
            return Ok(Users);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostAsync([FromBody] PostViewModel model)
        {
            AspNetUser User = Mapper.Map<AspNetUser>(model);
            (FeedBack, AspNetUser) Feed = await PostUserComposAsync(User);
            if (Feed.Item1 == FeedBack.AddedSuccess)
            {
                await UnitOfWork.UserRoleService.PostAsync(Feed.Item2, nameof(model.Role));
            }
            return Ok(Response(Feed.Item1, Mapper.Map<UserViewModel>(Feed.Item2)));
        }
        [HttpPut]
        //[Authorize(Roles = "Admin, NormalUser")]
        public async Task<ActionResult> PutAsync([FromBody] PutViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(CurrentUser());
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            CurrentFile = User.UserImage;
            var MapedUpdateAccountInUser = Mapper.Map<PutViewModel, AspNetUser>(model, User);
            (FeedBack, AspNetUser) Feed = await PutUserComposAsync(MapedUpdateAccountInUser, CurrentFile);
            return Ok(Response(Feed.Item1, Mapper.Map<UserViewModel>(Feed.Item2)));
        }
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel model)
        {
            AspNetUser User = await UnitOfWork.UserAuthService.FindAsync(CurrentUser());
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            var MapedChangePasswordInUser = Mapper.Map<ChangePasswordViewModel, AspNetUser>(model, User);
            string NewPassword = UnitOfWork.UserAuthService.PasswordHasher(User);
            MapedChangePasswordInUser.PasswordHash = NewPassword;
            FeedBack Feed = await UnitOfWork.UserAuthService.PutAsync(MapedChangePasswordInUser);
            return Ok(Response(Feed));
        }
        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordViewModel model)
        {
            var Model = await UnitOfWork.UserAuthService.FindByEmailAsync(model.Email);
            if (Model == null)
                return Ok(Response(FeedBack.NullOrEmpty));
            var code = await UnitOfWork.UserAuthService.GeneratePasswordResetTokenAsync(Model);
            var body = $"Please reset your password by copy this code: {code}";
            FeedBack Feed = UnitOfWork.HttpClientService.Mailer(new MailerDominModel { Sender = "info@dsmc.com.sa", Receiver = Model.Email, Host = "mail.dsmc.com.sa", Password = "DsmcBooking@2020", MessageBody = body }, EnumMailer.ResetPassword);
            return Ok(Response(Feed));
        }
        [HttpPut]
        [Route("ConfirmPassword")]
        public async Task<ActionResult> ConfirmPasswordAsync(NewPasswordViewModel model)
        {
            var Model = await UnitOfWork.UserAuthService.FindByEmailAsync(model.Email);
            if (Model == null)
                return Ok(Response(FeedBack.NullOrEmpty));
            await UnitOfWork.UserAuthService.ResetPasswordAsync(Model, model.Code, model.Password);
            return Ok();
        }
        [HttpPost]
        [Route("Token")]
        [AllowAnonymous]
        public async Task<ActionResult> GenerateTokenAsync([FromBody] LoginViewModel model)
        {
            if (CurrentConsumer() == EnumConsumer.DeviceConsumer)
                return Unauthorized(Response(FeedBack.Unauthorized));
            AspNetUser User = await UnitOfWork.UserAuthService.FindByNameAsync(model.UserName);
            if (User is null)
                return Ok(Response(FeedBack.NotFound));
            string Role = (await UnitOfWork.UserRoleService.GetAsync(User)).FirstOrDefault();
            if (string.IsNullOrEmpty(Role))
                return Ok(Response(FeedBack.Unauthorized));
            bool CheckPassword = await UnitOfWork.UserAuthService.AnyPasswordAsync(User, model.Password);
            if (CheckPassword == false)
                return Ok(Response(FeedBack.LoginFail));
            User.DefaultRole = Role;
            var MapRow = Mapper.Map<JwtTokenViewModel>(User);
            return Ok(Response(FeedBack.LoginSuccess, GenerateJwtToken(MapRow)));
        }
        [HttpPost]
        [Route("GenerateOTP")]
        [AllowAnonymous]
        public async Task<ActionResult> GenerateOTPAsync([FromBody] OTPViewModel model)
        {
            AspNetUser CurrentUser = await UnitOfWork.UserAuthService.FindByPhoneNumberAsync(model.PhoneNumber);
            if (CurrentUser is null)
            {
                PostViewModel MapedOTPViewModelInRegestrationViewModel = Mapper.Map<OTPViewModel, PostViewModel>(model);
                AspNetUser MapedRegestrationViewModelInAspNetUserModel = Mapper.Map<PostViewModel, AspNetUser>(MapedOTPViewModelInRegestrationViewModel, CurrentUser);
                (FeedBack, AspNetUser) Feed = await RegestrationUserAutomaticComposAsync(MapedRegestrationViewModelInAspNetUserModel, nameof(EnumUserRole.Employee));
                if (Feed.Item1 != FeedBack.AddedSuccess)
                    return Ok(Response(Feed.Item1));
                object CodeNumber = await FeatureManager.IsEnabledAsync(feature: "ForTest") ? (await UnitOfWork.UserAuthService.FindAsync(Feed.Item2.Id)).OTPCode.ToString() : "0";
                return Ok(Response(FeedBack.OTPGenerated, CodeNumber));
            }
            else
            if (CurrentUser is not null && CurrentUser.DefaultRole == ((int)EnumUserRole.Employee).ToString())
            {
                var InRole = await UnitOfWork.UserRoleService.AnyUserInRoleAsync(CurrentUser, nameof(EnumUserRole.Employee));
                if (!InRole)
                    return Unauthorized(Response(FeedBack.Unauthorized));
                FeedBack PutedFeedBack = await GenerateOTPComposAsync(CurrentUser);
                if (PutedFeedBack != FeedBack.OTPGenerated)
                    return Ok(Response(PutedFeedBack));
                object CodeNumber = await FeatureManager.IsEnabledAsync(feature: "ForTest") ? (await UnitOfWork.UserAuthService.FindByPhoneNumberAsync(model.PhoneNumber)).OTPCode.ToString() : "0";
                return Ok(Response(PutedFeedBack, CodeNumber));
            }
            else
            {
                return Ok(Response(FeedBack.Unauthorized));
            }
        }
        [HttpPost]
        [Route("VerifyOTP")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyOTPAsync([FromBody] VerifyOTPViewModel model)
        {
            AspNetUser CurrentUser = await UnitOfWork.UserAuthService.FindByPhoneNumberAsync(model.PhoneNumber);
            if (CurrentUser is null)
                return Ok(Response(FeedBack.NotFound));
            var User = Mapper.Map<VerifyOTPViewModel, AspNetUser>(model, CurrentUser);
            string Role = (await UnitOfWork.UserRoleService.GetAsync(User)).FirstOrDefault();
            if (string.IsNullOrEmpty(Role))
                return Ok(Response(FeedBack.Unauthorized));
            FeedBack PutedFeedBack = await VerifyOTPComposAsync(User);
            if (PutedFeedBack != FeedBack.AccountVerified)
                return Ok(Response(PutedFeedBack));
            User.DefaultRole = Role;
            var MapRow = Mapper.Map<JwtTokenViewModel>(User);
            return Ok(Response(PutedFeedBack, GenerateJwtToken(MapRow)));
        }
        [HttpPost]
        [Route("FirebaseToken")]
        [Authorize(Roles = "Admin, NormalUser")]
        public async Task<ActionResult> FirebaseTokenAsync([FromBody] FirebaseTokenModel model)
        {
            AspNetUser currentUser = await UnitOfWork.UserAuthService.FindAsync(CurrentUser());
            if (currentUser is null)
            {
                return Ok(Response(FeedBack.NotFound));
            }
            else
            {
                AspNetUser User = Mapper.Map<FirebaseTokenModel, AspNetUser>(model, currentUser);
                FeedBack Feed = await UnitOfWork.UserAuthService.PutAsync(User);
                return Ok(Response(Feed));
            }
        }

        #region Compos Funcation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<FeedBack> GenerateOTPComposAsync(AspNetUser model)
        {
            if (!UnitOfWork.UserAuthService.AnyByPhoneNumberAndAccountStatus(model.PhoneNumber, true))
                return FeedBack.AccountIsBlocked;
            double Total = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.SaudiArab).Subtract(model.OTPDate).TotalMinutes;
            if (Total <= 1)
                return FeedBack.UnUsedCode;
            model.OTPCode = Convert.ToInt32(UnitOfWork.GenerateRandomService.Random4Digit());
            model.OTPIsUsed = false;
            model.OTPDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.SaudiArab);
            FeedBack PutedFeedBack = await UnitOfWork.UserAuthService.PutAsync(model);
            if (PutedFeedBack != FeedBack.EditedSuccess)
                    await UnitOfWork.HttpClientService.SendSMSCode(model.PhoneNumber, "Your Code", model.OTPCode.ToString());
            return PutedFeedBack == FeedBack.EditedSuccess ? FeedBack.OTPGenerated : FeedBack.OTPGeneratedFail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<FeedBack> VerifyOTPComposAsync(AspNetUser user)
        {
            if (UnitOfWork.UserAuthService.AnyByPhoneNumberAndOTPCode(user.PhoneNumber, (int)user.OTPCode) == false)
                return FeedBack.CodeInCorrect;
            if (UnitOfWork.UserAuthService.AnyByPhoneNumberAndOTPCodeAndOTPUsed(user.PhoneNumber, (int)user.OTPCode, false) == false)
                return FeedBack.CodeNotUsed;
            user.OTPIsUsed = true;
            FeedBack PutedFeedBack = await UnitOfWork.UserAuthService.PutAsync(user);
            return PutedFeedBack == FeedBack.EditedSuccess ? FeedBack.AccountVerified : FeedBack.AccountNotVerified;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<(FeedBack, AspNetUser)> RegestrationUserAutomaticComposAsync(AspNetUser model, string role)
        {
            model.DefaultRole = ((int)EnumUserRole.Employee).ToString();
            model.OTPCode = int.Parse(UnitOfWork.GenerateRandomService.Random4Digit());
            model.OTPDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.SaudiArab);
            model.DateCreated = model.OTPDate; model.OTPIsUsed = false;
            (FeedBack, AspNetUser) Result = await UnitOfWork.UserAuthService.PostAsync(model);
            if (Result.Item1 == FeedBack.AddedSuccess)
            {
                await UnitOfWork.UserRoleService.PostAsync(Result.Item2, role);
                await UnitOfWork.HttpClientService.SendSMSCode(model.PhoneNumber, "Your Code", model.OTPCode.ToString());
            }
            return Result;
        }
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
            (FeedBack, AspNetUser) Feed = await UnitOfWork.UserAuthService.PostAsync(model);
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
