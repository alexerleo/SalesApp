using Microsoft.AspNet.Identity.EntityFramework;
using Sales.DataModel.Companies;
using Sales.DataModel.Mobile;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataModel.DbConfig
{
    public class SalesDbContext : IdentityDbContext<Admin>
    {
        public SalesDbContext() : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasRequired(q => q.Company).WithMany(q => q.Employees).HasForeignKey(q => q.CompanyId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Button>().HasRequired(q => q.Company).WithMany(q => q.Buttons).HasForeignKey(q => q.CompanyId).WillCascadeOnDelete();
            modelBuilder.Entity<Click>().HasRequired(q => q.Button).WithMany(q => q.Clicks).HasForeignKey(q => q.ButtonId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Click>().HasRequired(q => q.Employee).WithMany(q => q.Clicks).HasForeignKey(q => q.EmployeeId).WillCascadeOnDelete(false);
            modelBuilder.Entity<SessionToken>().HasRequired(t => t.Employee).WithMany(q => q.SessionTokens).HasForeignKey(q => q.EmployeeId).WillCascadeOnDelete(false);
        }
        public DbSet<Button> Buttons { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Click> Clicks { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
    }
}
