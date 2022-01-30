using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OA.Dtos.ServiceViewModel
{
    public abstract class SheardEvaluationViewModel
    {
        [Required]
        public short Value { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
    public class EvaluationViewModel : SheardEvaluationViewModel
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentNameUr { get; set; }
        public string UserName { get; set; }
        public string UserNameAr { get; set; }
    }
    public class MobileEvaluationViewModel : SheardEvaluationViewModel
    {
        public int Id { get; set; }
    }
    public class PostEvaluationViewModel : SheardEvaluationViewModel
    {
        public string UserId { get; set; }
        public short? DepartmentId { get; set; }
    }
    public class PutEvaluationViewModel : SheardEvaluationViewModel
    {
        
    }
}
