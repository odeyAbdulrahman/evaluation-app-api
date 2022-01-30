using AutoMapper;
using OA.Api.ConfigureServices;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;

namespace OA.Api.Profiles
{
    public class EvaluationProfile : Profile
    {
        public EvaluationProfile()
        {
            //From View ==> To View

            //From View ==> To Class
            CreateMap<PostEvaluationViewModel, Evaluation>();
            CreateMap<PutEvaluationViewModel, Evaluation>();
            //From Class ==> To View
            CreateMap<Evaluation, EvaluationViewModel>().
            ConstructUsing(x => new EvaluationViewModel
            {
                DepartmentName = x.Department != null ? x.Department.Name : "",
                DepartmentNameAr = x.Department != null ? x.Department.NameAr : "",
                DepartmentNameUr = x.Department != null ? x.Department.NameUr : "",
                UserName = x.User != null ? x.User.FullName : "",
                UserNameAr = x.User != null ? x.User.FullNameAr : ""
            });
            CreateMap<Evaluation, MobileEvaluationViewModel>();
        }
    }
}
