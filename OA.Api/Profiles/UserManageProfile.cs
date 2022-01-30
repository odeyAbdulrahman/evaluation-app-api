using AutoMapper;
using OA.Api.Common;
using OA.Api.ConfigureServices;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;

namespace OA.Api.Profiles
{
    public class UserManageProfile : Profile
    {
        public UserManageProfile()
        {
            //From View ==> To View
            CreateMap<PostUserManageViewModel, AspNetUser>();
            CreateMap<PutUserManageViewModel, AspNetUser>();
            CreateMap<ChangePasswordViewModel, AspNetUser>();
            CreateMap<ArchiveUserManageViewModel, AspNetUser>();
            CreateMap<AvailabilityStatusUserManageViewModel, AspNetUser>();
            //From View ==> To Class
            CreateMap<AspNetUser, UserManageViewModel>().
            ConstructUsing(x => new UserManageViewModel
            {
            });
            //From Class ==> To View
        }
    }
}
