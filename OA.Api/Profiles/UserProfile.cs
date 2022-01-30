using AutoMapper;
using OA.Api.Common;
using OA.Api.ConfigureServices;
using OA.Base.Enums;
using OA.Base.Helpers;
using OA.Data.Models;
using OA.Dtos.ServiceViewModel;

namespace OA.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //From View ==> To View
            CreateMap<OTPViewModel, PostViewModel>().ConstructUsing( x => new PostViewModel
            {
                UserName = x.PhoneNumber,
                FullName = x.PhoneNumber,
                FullNameAr = x.PhoneNumber,
                Email = string.Concat(x.PhoneNumber, "@User.com"),
            });

            //From View ==> To Class
            CreateMap<OTPViewModel, AspNetUser>();
            CreateMap<VerifyOTPViewModel, AspNetUser>();
            CreateMap<PostViewModel, AspNetUser>().ConstructUsing(x => new AspNetUser
            {
                DefaultRole = ((long)x.Role).ToString(),
                AvailabilityStatus = x.Role == EnumUserRole.User ? true : false
            });
            CreateMap<PutViewModel, AspNetUser>();
            CreateMap<FirebaseTokenModel, AspNetUser>();
            
            //From Class ==> To View
            CreateMap<AspNetUser, UserViewModel>().
            ConstructUsing(x => new UserViewModel
            {
            });
            CreateMap<AspNetUser, JwtTokenViewModel>().ConstructUsing(x => new JwtTokenViewModel
            {
                UserId = x.Id,
                Role = x.DefaultRole
            });
        }
    }
}
