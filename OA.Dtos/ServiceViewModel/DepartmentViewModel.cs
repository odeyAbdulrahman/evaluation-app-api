using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OA.Dtos.ServiceViewModel
{
    public abstract class SheardDepartmentViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NameUr { get; set; }

    }
    public class DepartmentViewModel : SheardDepartmentViewModel
    {
        public short Id { get; set; }
        public string DepartmentHeadName { get; set; }
        public string DepartmentHeadNameAr { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class MobileDepartmentViewModel : SheardDepartmentViewModel
    {
        public int Id { get; set; }
    }
    public class PostDepartmentViewModel : SheardDepartmentViewModel
    {
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
    public class PutDepartmentViewModel : SheardDepartmentViewModel
    {
        [JsonIgnore]
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }
    }
    public class HeadDepartmentViewModel 
    {
        public short DepartmentId { get; set; }
        public string UserId { get; set; }
    }
}
