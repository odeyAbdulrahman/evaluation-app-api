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
    public class DepartmentController : BaseController
    {
        public DepartmentController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper) : base(unitOfWork, appSettings)
        {
            Mapper = mapper;
        }
        private readonly IMapper Mapper;
        public static short? Id { get; private set; }
        public static DateTime From { get; private set; }
        public static DateTime To { get; private set; }
        public static Department Model { get; private set; }
        public static (FeedBack, Department) Feed { get; private set; }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync([FromRoute] short? id)
        {
            Id = id;
            if (Id == null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.DepartmentService.FirstOrDefaultAsync(filter: x => x.Id == (int)Id, x => x.User, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<DepartmentViewModel>(Model));
            return Ok(Mapper.Map<MobileDepartmentViewModel>(Model));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var Models = await UnitOfWork.DepartmentService.ListAsync(filter: x => x.Id != 1, orderBy: x => x.OrderBy(xx => xx.Name), x => x.User, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentViewModel>>(Models));
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult> GetAsync(int skip, int take)
        {
            var Models = await UnitOfWork.DepartmentService.ListAsync(skip, take, filter: x => x.Id != 1, orderBy: x => x.OrderBy(xx => xx.Name), x => x.User, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentViewModel>>(Models));
        }
        [HttpGet("{from}/{to}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(DateTime from, DateTime to, int skip, int take)
        {
            From = from; To = to;
            var Models = await UnitOfWork.DepartmentService.ListAsync(skip, take, filter: x => x.Id != 1 && x.CreatedDate >= From && x.CreatedDate <= To, orderBy: x => x.OrderBy(xx => xx.Name), x => x.User, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentViewModel>>(Models));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PostDepartmentViewModel model)
        {
            model.CreatedBy = CurrentUser(); model.CreatedDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            var MapViewModelInModel = Mapper.Map<Department>(model);
            Feed = await UnitOfWork.DepartmentService.PostAsync(MapViewModelInModel);
            if (Feed.Item1 == FeedBack.AddedSuccess)
            {
                Model = await UnitOfWork.DepartmentService.FirstOrDefaultAsync(filter: x => x.Id == Feed.Item2.Id, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
                if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                    return Ok(Response(Feed.Item1, Mapper.Map<DepartmentViewModel>(Model)));
                return Ok(Response(Feed.Item1, Mapper.Map<MobileDepartmentViewModel>(Model)));
            }
            else
            {
                return Ok(Response(Feed.Item1));
            }
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutAsync([FromBody] PutDepartmentViewModel model, short? id)
        {
            Id = id;
            if (Id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.DepartmentService.FirstOrDefaultAsync(filter: x => x.Id == (int)Id, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            model.UpdatedBy = CurrentUser(); model.UpdatedDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            Department MapViewModelInModel = Mapper.Map<PutDepartmentViewModel, Department>(model, Model);
            (FeedBack, Department) Feed = await UnitOfWork.DepartmentService.UpdateAsync(MapViewModelInModel);
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Response(Feed.Item1, Mapper.Map<DepartmentViewModel>(Feed.Item2)));
            return Ok(Response(Feed.Item1, Mapper.Map<MobileDepartmentViewModel>(Feed.Item2)));
        }
        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody] HeadDepartmentViewModel model)
        {
            Model = await UnitOfWork.DepartmentService.FirstOrDefaultAsync(filter: x => x.Id == model.DepartmentId, x => x.ApplicationUserCreatedBy, x => x.ApplicationUserUpdatedBy);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            Department MapViewModelInModel = Mapper.Map<HeadDepartmentViewModel, Department>(model, Model);
            (FeedBack, Department) Feed = await UnitOfWork.DepartmentService.UpdateAsync(MapViewModelInModel);
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Response(Feed.Item1, Mapper.Map<DepartmentViewModel>(Feed.Item2)));
            return Ok(Response(Feed.Item1, Mapper.Map<MobileDepartmentViewModel>(Feed.Item2)));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DelAsync(short? id)
        {
            if (id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.DepartmentService.FindAsync((short)id);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            FeedBack Feed = await UnitOfWork.DepartmentService.DeleteAsync(Model);
            return Ok(Response(Feed));
        }
        #region Helper

        #endregion
    }
}
