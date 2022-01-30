using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Models
{
    public class AspNetUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        public string FullNameAr { get; set; }
        public string UserImage { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        public int? OTPCode { get; set; }
        public bool? OTPIsUsed { get; set; }
        public DateTime OTPDate { get; set; }
        public bool? AvailabilityStatus { get; set; }
        public bool? MobileStatus { get; set; }
        public bool? Archive { get; set; }
        public bool? IsBusy { get; set; }
        public string FirebaseToken { get; set; }
        [Required]
        public double Blance { get; set; }
        public string DefaultRole { get; set; }
        [StringLength(5)]
        public string CountryCode { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<Department> DepartmentsCreated { get; set; }
        public ICollection<Department> DepartmentsUpdated { get; set; }
        public ICollection<DepartmentEmployee> DepartmentEmployeesCreated { get; set; }
        public ICollection<DepartmentEmployee> DepartmentEmployeesUpdated { get; set; }
    }
}
