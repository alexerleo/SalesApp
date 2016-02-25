namespace Sales.DataModel.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Sales.DataModel.DbConfig;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Sales.DataModel.DbConfig.SalesDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SalesDbContext context)
        {
            if (context.Users.FirstOrDefault(x => x.UserName == "MainAdmin") == null)
            {
                AddDefaultRoles(context);
                AddDefaultAdmin(context);
            }
            base.Seed(context);
        }
        private void AddDefaultRoles(SalesDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var mainAdminRole = new IdentityRole { Name = RoleNames.MainAdmin };
            var AdminRole = new IdentityRole { Name = RoleNames.Admin };
            roleManager.Create(mainAdminRole);
            roleManager.Create(AdminRole);
            context.SaveChanges();
        }
        private void AddDefaultAdmin(SalesDbContext context)
        {
            var userManager = new UserManager<Admin>(new UserStore<Admin>(context));
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("123456");
            var mainAdmin = new Admin { Email = "main_admin@gmail.com", UserName = "MainAdmin", PasswordHash = password, SecurityStamp = Guid.NewGuid().ToString("D") };
            context.Users.Add(mainAdmin);
            context.SaveChanges();
            userManager.AddToRole(mainAdmin.Id, RoleNames.MainAdmin);
        }
    }
}
