using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OA.Data.Maps;
using OA.Data.Models;
using System;

namespace OA.Data
{
    public class AppDbContext : IdentityDbContext<AspNetUser>
    {
        //const string MyConnectionString = "Server=sql6012.site4now.net;Database=db_a4063b_eval;Uid=db_a4063b_eval_admin;Pwd=Cc#eval-app";
        //const string MyConnectionString = "Data Source=SQL6012.site4now.net;Initial Catalog=db_a4063b_eval;User Id=db_a4063b_eval_admin;Password=Cc#eval1";
        //public AppDbContext()
        //{
        //}

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AspNetUser> ApplicationUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployees { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(MyConnectionString);
        //}

        [Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = new AspNetUserMap(modelBuilder.Entity<AspNetUser>());
            _ = new DepartmentMap(modelBuilder.Entity<Department>());
            _ = new DepartmentEmployeeMap(modelBuilder.Entity<DepartmentEmployee>());
            _ = new EvaluationMap(modelBuilder.Entity<Evaluation>());

        }
    }
}
