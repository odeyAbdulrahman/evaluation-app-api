using OA.Base.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OA.Dtos.ServiceViewModel
{

    public abstract class SheardUserManageViewModel
    {
        [Required]
        [StringLength(10)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        public string FullNameAr { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(5)]
        public string CountryCode { get; set; } = "+971";
        [Required]
        [StringLength(9)]
        public string PhoneNumber { get; set; }
        public string UserImage { get; set; }
    }
    public class UserManageViewModel: SheardUserManageViewModel
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public bool AvailabilityStatus { get; set; }
        public bool MobileStatus { get; set; }
        public bool Busy { get; set; }
        public bool Archive { get; set; }
        public string DefaultRole { get; set; }
    }
    public class PostUserManageViewModel: SheardUserManageViewModel
    {
        public bool? AvailabilityStatus { get; set; } = true;
        public EnumUserRole Role { get; set; }
        public string PasswordHash { get; set; }
    }
    public class PutUserManageViewModel : SheardUserManageViewModel
    {
        public string Id { get; set; }
        public bool? AvailabilityStatus { get; set; } = false;
    }
    public class ArchiveUserManageViewModel
    {
        public string Id { get; set; }
        public bool? Archive { get; set; }
    }
    public class AvailabilityStatusUserManageViewModel
    {
        public string Id { get; set; }
        public bool? AvailabilityStatus { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
    }
    public class NewPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }
        public string PasswordHash { get; set; }
    }
}
