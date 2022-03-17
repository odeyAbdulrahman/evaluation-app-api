using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Data.Models
{
    public class Department: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NameUr { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public virtual ICollection<SubDepartment> SubDepartments { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
