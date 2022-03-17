using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Data.Maps
{
    public class EvaluationMap
    {
        [Obsolete]
        public EvaluationMap(EntityTypeBuilder<Evaluation> entityBuilder)
        {
            entityBuilder.HasOne(s => s.Department)
                         .WithMany(u => u.Evaluations)
                         .HasForeignKey(s => s.DepartmentId)
                         .OnDelete(DeleteBehavior.Restrict)
                         .IsRequired(false);

            entityBuilder.HasOne(s => s.SubDepartment)
                         .WithMany(u => u.Evaluations)
                         .HasForeignKey(s => s.SubDepartmentId)
                         .OnDelete(DeleteBehavior.Restrict)
                         .IsRequired(false);

            entityBuilder.HasOne(s => s.User)
                .WithMany(u => u.Evaluations)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
