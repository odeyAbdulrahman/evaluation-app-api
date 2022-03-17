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
    public class DepartmentEmployeeController : BaseController
    {
        public DepartmentEmployeeController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper) : base(unitOfWork, appSettings)
        {
            Mapper = mapper;
        }
        private readonly IMapper Mapper;
        public static int? Id { get; private set; }
        public static short? DepartId { get; private set; }
        public static DateTime From { get; private set; }
        public static DateTime To { get; private set; }
        public static DepartmentEmployee Model { get; private set; }
        public static (FeedBack, DepartmentEmployee) Feed { get; private set; }

        [HttpGet("{deptId}")]
        public async Task<ActionResult> GetAsync(short? deptId)
        {
            DepartId = deptId;
            var Models = await UnitOfWork.DepartmentEmployeeService.ListAsync(filter: x => x.DepartmentId >= DepartId, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentEmployeeViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentEmployeeViewModel>>(Models));
        }
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var Models = await UnitOfWork.DepartmentEmployeeService.ListAsync(null, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentEmployeeViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentEmployeeViewModel>>(Models));
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult> GetAsync(int skip, int take)
        {
            var Models = await UnitOfWork.DepartmentEmployeeService.ListAsync(skip, take, null, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentEmployeeViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentEmployeeViewModel>>(Models));
        }
        [HttpGet("{deptId}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(short deptId, int skip, int take)
        {
            DepartId = deptId;
            var Models = await UnitOfWork.DepartmentEmployeeService.ListAsync(skip, take, filter: x => x.DepartmentId >= DepartId, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentEmployeeViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentEmployeeViewModel>>(Models));
        }
        [HttpGet("{from}/{to}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(DateTime from, DateTime to, int skip, int take)
        {
            From = from; To = to;
            var Models = await UnitOfWork.DepartmentEmployeeService.ListAsync(skip, take, filter: x => x.CreatedDate >= From && x.CreatedDate <= To, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<DepartmentEmployeeViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileDepartmentEmployeeViewModel>>(Models));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PostDepartmentEmployeeViewModel model)
        {
            Model = await UnitOfWork.DepartmentEmployeeService.FirstOrDefaultAsync(filter: x => x.DepartmentId == model.DepartmentId && x.UserId == model.UserId);
            if (Model is not null)
                return Ok(Response(FeedBack.IsExist));
            model.CreatedBy = CurrentUser();
            model.CreatedDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            var MapViewModelInModel = Mapper.Map<DepartmentEmployee>(model);
            Feed = await UnitOfWork.DepartmentEmployeeService.PostAsync(MapViewModelInModel);
            if (Feed.Item1 == FeedBack.AddedSuccess)
            {
                Model = await UnitOfWork.DepartmentEmployeeService.FirstOrDefaultAsync(filter: x => x.Id == Feed.Item2.Id, x => x.User, x => x.Department);
                if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                    return Ok(Response(Feed.Item1, Mapper.Map<DepartmentEmployeeViewModel>(Model)));
                return Ok(Response(Feed.Item1, Mapper.Map<MobileDepartmentEmployeeViewModel>(Model)));
            }
            else
            {
                return Ok(Response(Feed.Item1));
            }
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutAsync([FromBody] PutDepartmentEmployeeViewModel model, int? id)
        {
            Id = id;
            if (Id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.DepartmentEmployeeService.FirstOrDefaultAsync(filter: x => x.Id == (int)Id, x => x.User, x => x.Department);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            DepartmentEmployee MapViewModelInModel = Mapper.Map<PutDepartmentEmployeeViewModel, DepartmentEmployee>(model, Model);
            MapViewModelInModel.UpdatedBy = CurrentUser();
            MapViewModelInModel.UpdatedDate = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.SaudiArab);
            (FeedBack, DepartmentEmployee) Feed = await UnitOfWork.DepartmentEmployeeService.UpdateAsync(MapViewModelInModel);
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Response(Feed.Item1, Mapper.Map<DepartmentEmployeeViewModel>(Feed.Item2)));
            return Ok(Response(Feed.Item1, Mapper.Map<MobileDepartmentEmployeeViewModel>(Feed.Item2)));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DelAsync(int? id)
        {
            if (id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.DepartmentEmployeeService.FindAsync((int)id);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            FeedBack Feed = await UnitOfWork.DepartmentEmployeeService.DeleteAsync(Model);
            return Ok(Response(Feed));
        }
        #region Helper

        #endregion
    }
}
