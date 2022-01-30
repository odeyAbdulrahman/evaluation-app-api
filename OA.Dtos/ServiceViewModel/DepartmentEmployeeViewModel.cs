using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OA.Dtos.ServiceViewModel
{
    public abstract class SheardDepartmentEmployeeViewModel
    {
        public short DepartmentId { get; set; }
        public string UserId { get; set; }
    }
    public class DepartmentEmployeeViewModel : SheardDepartmentEmployeeViewModel
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentNameUr { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameAr { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class MobileDepartmentEmployeeViewModel : SheardDepartmentEmployeeViewModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameAr { get; set; }
    }
    public class PostDepartmentEmployeeViewModel : SheardDepartmentEmployeeViewModel
    {
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
    public class PutDepartmentEmployeeViewModel : SheardDepartmentEmployeeViewModel
    {
        [JsonIgnore]
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }
    }
}
