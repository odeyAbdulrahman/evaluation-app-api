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
    public class DepartmentMap
    {
        [Obsolete]
        public DepartmentMap(EntityTypeBuilder<Department> entityBuilder)
        {
            entityBuilder.HasIndex(t => t.Name).HasName("Index_Name_Unicode").IsUnique();
            entityBuilder.HasIndex(t => t.NameAr).HasName("Index_NameAr_Unicode").IsUnique();
            entityBuilder.HasOne(s => s.ApplicationUserCreatedBy)
                 .WithMany(u => u.DepartmentsCreated)
                 .HasForeignKey(s => s.CreatedBy)
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired(false);
            entityBuilder.HasOne(s => s.ApplicationUserUpdatedBy)
                .WithMany(u => u.DepartmentsUpdated)
                .HasForeignKey(s => s.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
