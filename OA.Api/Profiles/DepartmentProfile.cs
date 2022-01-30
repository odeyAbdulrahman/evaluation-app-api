using AutoMapper;
using OA.Api.ConfigureServices;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;

namespace OA.Api.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            //From View ==> To View

            //From View ==> To Class
            CreateMap<PostDepartmentViewModel, Department>();
            CreateMap<PutDepartmentViewModel, Department>();
            CreateMap<HeadDepartmentViewModel, Department>();
            
            //From Class ==> To View
            CreateMap<Department, DepartmentViewModel>().
            ConstructUsing(x => new DepartmentViewModel
            {
                CreatedName = x.ApplicationUserCreatedBy != null ? x.ApplicationUserCreatedBy.FullName:"",
                UpdatedName = x.ApplicationUserUpdatedBy != null ? x.ApplicationUserUpdatedBy.FullName:"",
                DepartmentHeadName = x.User != null ? x.User.FullName:"",
                DepartmentHeadNameAr = x.User != null ? x.User.FullNameAr:"",
            });

            CreateMap<Department, MobileDepartmentViewModel>();
        }
    }
}
