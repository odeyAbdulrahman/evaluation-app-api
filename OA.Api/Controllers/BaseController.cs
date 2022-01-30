using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OA.Api.UnitOfWork;
using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Base.Messages;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        protected IUnitOfWork UnitOfWork;
        protected AppSettings AppSettings;
        public BaseController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            UnitOfWork = unitOfWork;
            AppSettings = appSettings.Value;
        }
        protected SystemMessagesEn Message = new SystemMessagesEn();
        protected SystemMessagesAr MessageAr = new SystemMessagesAr();
        protected string CurrentFile { get; set; }
        protected string CurrentFile2 { get; set; }

        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected EnumConsumer CurrentConsumer()
        {
            return (EnumConsumer)long.Parse(Request.Headers["Consumer"]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected EnumLang CurrentLang()
        {
            if (Request.Headers.ContainsKey("Lang"))
                if (Request.Headers["Lang"] != "")
                    return (EnumLang)int.Parse(Request.Headers["Lang"]);
            return EnumLang.Ar;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Feed"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected new object Response(FeedBack Feed)
        {
            return new { Code = Feed, Description = CurrentLang() == EnumLang.Ar ? MessageAr.YourMessage(Feed) : Message.YourMessage(Feed) };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Feed"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected new object Response(FeedBack Feed, object model)
        {
            return new { Code = Feed, Description = CurrentLang() == EnumLang.Ar ? MessageAr.YourMessage(Feed) : Message.YourMessage(Feed), Model = model };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected string CurrentUser()
        {
            ClaimsPrincipal CurrentUser = this.User;
            return UnitOfWork.UserAuthService.GetUserId(CurrentUser);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<AspNetUser> OrganizationOfUserAsync()
        {
            return await UnitOfWork.UserAuthService.FindByNameAsync(this.User.Identity.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected string GenerateJwtToken(JwtTokenViewModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            IdentityOptions _options = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, model.UserId),
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(_options.ClaimsIdentity.RoleClaimType, model.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }
        #endregion
    }
}
