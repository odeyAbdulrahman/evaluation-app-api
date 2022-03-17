using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

namespace evaluation_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ConsumerFilter]
    [Authorize]
    public class SubDepartmentController : BaseController
    {
        public SubDepartmentController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper) : base(unitOfWork, appSettings)
        {
            Mapper = mapper;
        }
        private readonly IMapper Mapper;
        public static short? Id { get; private set; }
        public static DateTime From { get; private set; }
        public static DateTime To { get; private set; }
        public static SubDepartment Model { get; private set; }
        public static (FeedBack, SubDepartment) Feed { get; private set; }

        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var Models = await UnitOfWork.SubDepartmentService.ListAsync(filter: x => x.Id != 1, orderBy: x => x.OrderBy(xx => xx.Id), x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<SubDepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileSubDepartmentViewModel>>(Models));
        }
        [AllowAnonymous]
        [HttpGet("{departmentId}")]
        public async Task<ActionResult> GetAsync([FromRoute] short? departmentId)
        {
            Id = departmentId;
            if (Id == null)
                return Ok(Response(FeedBack.NotFound));
            var Models = await UnitOfWork.SubDepartmentService.ListAsync(filter: x=> x.DepartmentId == Id, orderBy: x => x.OrderBy(xx => xx.Id), x => x.Department, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<SubDepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileSubDepartmentViewModel>>(Models));
        }

        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult> GetAsync(int skip, int take)
        {
            var Models = await UnitOfWork.SubDepartmentService.ListAsync(skip, take,null, orderBy: x => x.OrderBy(xx => xx.Id),x => x.Department, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<SubDepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileSubDepartmentViewModel>>(Models));
        }
        [HttpGet("{from}/{to}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(DateTime from, DateTime to, int skip, int take)
        {
            From = from; To = to;
            var Models = await UnitOfWork.SubDepartmentService.ListAsync(skip, take, filter: x => 
            x.CreatedDate >= From && x.CreatedDate <= To, orderBy: x => x.OrderBy(xx => xx.Id), x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<SubDepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileSubDepartmentViewModel>>(Models));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PostSubDepartmentViewModel model)
        {
            model.CreatedBy = CurrentUser(); model.CreatedDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            var MapViewModelInModel = Mapper.Map<SubDepartment>(model);
            Feed = await UnitOfWork.SubDepartmentService.PostAsync(MapViewModelInModel);
            if (Feed.Item1 == FeedBack.AddedSuccess)
            {
                Model = await UnitOfWork.SubDepartmentService.FirstOrDefaultAsync(filter: x => x.Id == Feed.Item2.Id, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
                if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                    return Ok(Response(Feed.Item1, Mapper.Map<SubDepartmentViewModel>(Model)));
                return Ok(Response(Feed.Item1, Mapper.Map<MobileSubDepartmentViewModel>(Model)));
            }
            else
            {
                return Ok(Response(Feed.Item1));
            }
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutAsync([FromBody] PutSubDepartmentViewModel model, short? id)
        {
            Id = id;
            if (Id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.SubDepartmentService.FirstOrDefaultAsync(filter: x => x.Id == (int)Id, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            model.UpdatedBy = CurrentUser(); model.UpdatedDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            SubDepartment MapViewModelInModel = Mapper.Map<PutSubDepartmentViewModel, SubDepartment>(model, Model);
            (FeedBack, SubDepartment) Feed = await UnitOfWork.SubDepartmentService.UpdateAsync(MapViewModelInModel);
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Response(Feed.Item1, Mapper.Map<SubDepartmentViewModel>(Feed.Item2)));
            return Ok(Response(Feed.Item1, Mapper.Map<MobileSubDepartmentViewModel>(Feed.Item2)));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DelAsync(short? id)
        {
            if (id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.SubDepartmentService.FindAsync((short)id);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            FeedBack Feed = await UnitOfWork.SubDepartmentService.DeleteAsync(Model);
            return Ok(Response(Feed));
        }
        #region Helper

        #endregion
    }
}
