using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Models
{
    public class DepartmentEmployee: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public short DepartmentId { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public Department Department { get; set; }
    }
}
