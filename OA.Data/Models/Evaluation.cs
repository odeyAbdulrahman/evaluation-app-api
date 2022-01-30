using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Models
{
    public class Evaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public short Value { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public short? DepartmentId { get; set; }
        public AspNetUser User { get; set; }
        public Department Department { get; set; }

    }
}
