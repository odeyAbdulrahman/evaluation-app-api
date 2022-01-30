using AutoMapper;
using OA.Base.Helpers;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;

namespace OA.Api.Profiles
{
    public class DepartmentEmployeeProfile : Profile
    {
        public DepartmentEmployeeProfile()
        {
            //From View ==> To View

            //From View ==> To Class
            CreateMap<PostDepartmentEmployeeViewModel, DepartmentEmployee>();
            CreateMap<PutDepartmentEmployeeViewModel, DepartmentEmployee>();
            
            //From Class ==> To View
            CreateMap<DepartmentEmployee, DepartmentEmployeeViewModel>().
            ConstructUsing(x => new DepartmentEmployeeViewModel
            {
                DepartmentName = x.Department != null ? x.Department.Name : "",
                DepartmentNameAr =  x.Department != null ? x.Department.NameAr:"",
                DepartmentNameUr =  x.Department != null ? x.Department.NameUr:"",
                EmployeeName = x.User != null ? x.User.FullName :"",
                EmployeeNameAr = x.User != null ? x.User.FullNameAr :"",
                CreatedName = x.ApplicationUserCreatedBy != null ? x.ApplicationUserCreatedBy.FullName : "",
                UpdatedName = x.ApplicationUserUpdatedBy != null ? x.ApplicationUserUpdatedBy.FullName : ""
            });

            CreateMap<DepartmentEmployee, MobileDepartmentEmployeeViewModel>().ConstructUsing(x => new MobileDepartmentEmployeeViewModel
            {
                EmployeeName = x.User != null ? x.User.FullName : "",
                EmployeeNameAr = x.User != null ? x.User.FullNameAr : "",
            });
        }
    }
}
