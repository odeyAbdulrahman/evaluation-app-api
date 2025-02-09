﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Models
{
    public class SubDepartment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NameUr { get; set; }
        public short? DepartmentId { get; set; } = null;
        public Department Department { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
