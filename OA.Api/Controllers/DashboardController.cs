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
    public class DashboardController : BaseController
    {
        public DashboardController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper) : base(unitOfWork, appSettings)
        {
            Mapper = mapper;
        }
        private readonly IMapper Mapper;
        public static int DepartmentId { get; private set; }
        public static int SubDepartmentId { get; private set; }
        public static DateTime From { get; private set; }
        public static DateTime To { get; private set; }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var To = UnitOfWork.DateTimeService.GetCurrentDateTime((int)EnumTimeZones.Emarat);
            var From = To.Date;
            var AllModels = await UnitOfWork.EvaluationService.ListAsync(null, orderBy: x => x.OrderBy(xx => xx.Value));
            var Models = await UnitOfWork.EvaluationService.ListAsync(x => x.Date >= From && x.Date <= To.Date.AddDays(1),orderBy: x => x.OrderBy(xx => xx.Value));
            List<(int, int, int)> EvaluationData = new()
            {
                ((int)EnumEmojes.Like, Models.Count(x => x.Value == (int)EnumEmojes.Like), AllModels.Count(x => x.Value == (int)EnumEmojes.Like)),
                ((int)EnumEmojes.Good, Models.Count(x => x.Value == (int)EnumEmojes.Good), AllModels.Count(x => x.Value == (int)EnumEmojes.Good)),
                ((int)EnumEmojes.PissedMe, Models.Count(x => x.Value == (int)EnumEmojes.PissedMe), AllModels.Count(x => x.Value == (int)EnumEmojes.PissedMe))
            };
            return Ok(EvaluationData);
        }

        [HttpGet("{departmentId}/{subDepartmentId}/{from}/{to}")]
        public async Task<ActionResult> GetAsync(int departmentId, int subDepartmentId, string from, string to)
        {
            
            DepartmentId = departmentId; SubDepartmentId = subDepartmentId; From = DateTime.ParseExact(from, "dd-MM-yyyy", null); To = DateTime.ParseExact(to, "dd-MM-yyyy", null);
            var Models = subDepartmentId <= 0 ? await UnitOfWork.EvaluationService.ListAsync(x => x.DepartmentId == DepartmentId && x.Date >= From.Date && x.Date <= To.Date.AddDays(1), orderBy: x => x.OrderBy(xx => xx.Value))
            : await UnitOfWork.EvaluationService.ListAsync(x => x.DepartmentId == DepartmentId && x.SubDepartmentId == SubDepartmentId && x.Date >= From.Date && x.Date <= To.Date.AddDays(1), orderBy: x => x.OrderBy(xx => xx.Value));
            List<(int, int)> EvaluationData = new()
            {
                ((int)EnumEmojes.Like, Models.Count(x => x.Value == (int)EnumEmojes.Like)),
                ((int)EnumEmojes.Good, Models.Count(x => x.Value == (int)EnumEmojes.Good)),
                ((int)EnumEmojes.PissedMe, Models.Count(x => x.Value == (int)EnumEmojes.PissedMe))
            };
            return Ok(EvaluationData);
        }
    }
}
