using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OA.Dtos.ServiceViewModel
{
    public abstract class SheardSubDepartmentViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NameUr { get; set; }
        public short? DepartmentId { get; set; }

    }
    public class SubDepartmentViewModel : SheardSubDepartmentViewModel
    {
        public short Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class MobileSubDepartmentViewModel : SheardSubDepartmentViewModel
    {
        public int Id { get; set; }
    }
    public class PostSubDepartmentViewModel : SheardSubDepartmentViewModel
    {
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
    public class PutSubDepartmentViewModel : SheardSubDepartmentViewModel
    {
        [JsonIgnore]
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }
    }
}
