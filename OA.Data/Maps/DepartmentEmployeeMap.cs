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
    public  class DepartmentEmployeeMap
    {
        [Obsolete]
        public DepartmentEmployeeMap(EntityTypeBuilder<DepartmentEmployee> entityBuilder)
        {
            entityBuilder.HasOne(s => s.ApplicationUserCreatedBy)
             .WithMany(u => u.DepartmentEmployeesCreated)
             .HasForeignKey(s => s.CreatedBy)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired(false);

            entityBuilder.HasOne(s => s.ApplicationUserUpdatedBy)
                .WithMany(u => u.DepartmentEmployeesUpdated)
                .HasForeignKey(s => s.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
