using AutoMapper;
using OA.Api.ConfigureServices;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;

namespace OA.Api.Profiles
{
    public class SubDepartmentProfile : Profile
    {
        public SubDepartmentProfile()
        {
            //From View ==> To View

            //From View ==> To Class
            CreateMap<PostSubDepartmentViewModel, SubDepartment>();
            CreateMap<PutSubDepartmentViewModel, SubDepartment>();
            
            //From Class ==> To View
            CreateMap<SubDepartment, SubDepartmentViewModel>().
            ConstructUsing(x => new SubDepartmentViewModel
            {
                DepartmentName = x.Department != null ? x.Department.Name:"",
                DepartmentNameAr = x.Department != null ? x.Department.NameAr:"",
                CreatedName = x.ApplicationUserCreatedBy != null ? x.ApplicationUserCreatedBy.FullName:"",
                UpdatedName = x.ApplicationUserUpdatedBy != null ? x.ApplicationUserUpdatedBy.FullName:"",
            });

            CreateMap<SubDepartment, MobileSubDepartmentViewModel>();
        }
    }
}
