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
    public class EvaluationController : BaseController
    {
        public EvaluationController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper) : base(unitOfWork, appSettings)
        {
            Mapper = mapper;
        }
        private readonly IMapper Mapper;
        public static int? Id { get; private set; }
        public static DateTime From { get; private set; }
        public static DateTime To { get; private set; }
        public static Evaluation Model { get; private set; }
        public static (FeedBack, Evaluation) Feed { get; private set; }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int? id)
        {
            Id = id;
            if (Id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.EvaluationService.FirstOrDefaultAsync(filter: x => x.Id == (int)Id, x => x.User, x => x.Department);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<EvaluationViewModel>(Model));
            return Ok(Mapper.Map<MobileEvaluationViewModel>(Model));
        }
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            DepartmentEmployee Model = UnitOfWork.DepartmentEmployeeService.FirstOrDefaultAsync(filter: x => x.UserId == CurrentUser()).GetAwaiter().GetResult();
            var Models = await UnitOfWork.EvaluationService.ListAsync(filter: x=> x.DepartmentId == (Model != null ? Model.DepartmentId : -1), orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department, x => x.SubDepartment);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<EvaluationViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileEvaluationViewModel>>(Models));
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult> GetAsync(int skip, int take)
        {
            var Models = await UnitOfWork.EvaluationService.ListAsync(skip, take, null, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<EvaluationViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileEvaluationViewModel>>(Models));
        }
        [HttpGet("{from}/{to}/{skip}/{take}")]
        public async Task<ActionResult> GetAsync(DateTime from, DateTime to, int skip, int take)
        {
            From = from; To = to;
            var Models = await UnitOfWork.EvaluationService.ListAsync(skip, take, filter: x => x.Date >= From && x.Date <= To, orderBy: x => x.OrderBy(xx => xx.Id), x => x.User, x => x.Department);
            if (Models is null)
                return Ok(Response(FeedBack.NotFound));
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Mapper.Map<List<EvaluationViewModel>>(Models));
            return Ok(Mapper.Map<List<MobileEvaluationViewModel>>(Models));
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostAsync([FromBody] PostEvaluationViewModel model)
        {
            model.Date = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            model.UserId = model.UserId == "" ? (await UnitOfWork.UserAuthService.FindByNameAsync("Eval")).Id: model.UserId;
            var MapViewModelInModel = Mapper.Map<Evaluation>(model);
            Feed = await UnitOfWork.EvaluationService.PostAsync(MapViewModelInModel);
            if (Feed.Item1 == FeedBack.AddedSuccess)
            {
                Model = await UnitOfWork.EvaluationService.FirstOrDefaultAsync(filter: x => x.Id == Feed.Item2.Id, x => x.User, x => x.Department);
                if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                    return Ok(Response(Feed.Item1, Mapper.Map<EvaluationViewModel>(Model)));
                return Ok(Response(Feed.Item1, Mapper.Map<MobileEvaluationViewModel>(Model)));
            }
            else
            {
                return Ok(Response(Feed.Item1));
            }
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutAsync([FromBody] PutEvaluationViewModel model, int? id)
        {
            Id = id;
            if (Id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.EvaluationService.FirstOrDefaultAsync(filter: x => x.Id == (int)Id, x => x.User, x => x.Department);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            Evaluation MapViewModelInModel = Mapper.Map<PutEvaluationViewModel, Evaluation>(model, Model);
            (FeedBack, Evaluation) Feed = await UnitOfWork.EvaluationService.UpdateAsync(MapViewModelInModel);
            if (CurrentConsumer() == EnumConsumer.cPanelConsumer)
                return Ok(Response(Feed.Item1, Mapper.Map<EvaluationViewModel>(Feed.Item2)));
            return Ok(Response(Feed.Item1, Mapper.Map<MobileEvaluationViewModel>(Feed.Item2)));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DelAsync(long? id)
        {
            if (id is null)
                return Ok(Response(FeedBack.NotFound));
            Model = await UnitOfWork.EvaluationService.FindAsync((long)id);
            if (Model is null)
                return Ok(Response(FeedBack.NotFound));
            FeedBack Feed = await UnitOfWork.EvaluationService.DeleteAsync(Model);
            return Ok(Response(Feed));
        }
        #region Helper

        #endregion
    }
}
