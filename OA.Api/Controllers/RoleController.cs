using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OA.Api.Filters;
using OA.Api.UnitOfWork;
using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ConsumerFilter]
    [Authorize]
    public class RoleController : BaseController
    {
        public RoleController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings) : base(unitOfWork, appSettings)
        {
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(UnitOfWork.RolesDictionaryService.AddRoles(EnumTypeRole.All).GetRoles());
        }
        [HttpGet("{name}")]
        public async Task<ActionResult> GetAsync(string name)
        {
            bool Exist = await UnitOfWork.RoleService.RoleExistsAsync(name);
            if (Exist == false)
                return Ok(Response(FeedBack.NotFound));
            return Ok(await UnitOfWork.RoleService.FindByNameAsync(name));
        }
        [HttpGet("{type}/{orderBy}")]
        public ActionResult Get(EnumTypeRole type, EnumOrderBy orderBy)
        {
            return Ok(UnitOfWork.RolesDictionaryService.AddRoles(type).GetRoles());
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] string name)
        {
            bool Exist = await UnitOfWork.RoleService.RoleExistsAsync(name);
            if (Exist == true)
                return Ok(Response(FeedBack.IsExist));
            FeedBack Feed = await UnitOfWork.RoleService.PostAsync(new IdentityRole { Id = UnitOfWork.GenerateRandomService.RandomNumDigit(9), Name = name });
            return Ok(Response(Feed));
        }
        [HttpDelete]
        public async Task<ActionResult> DelAsync([FromBody] string name)
        {
            bool Exist = await UnitOfWork.RoleService.RoleExistsAsync(name);
            if (Exist == false)
                return Ok(Response(FeedBack.NotFound));
            IdentityRole role = await UnitOfWork.RoleService.FindByNameAsync(name);
            FeedBack Feed = await UnitOfWork.RoleService.DelAsync(role);
            return Ok(Response(Feed));
        }
    }
}
