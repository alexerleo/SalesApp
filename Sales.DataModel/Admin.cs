using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sales.DataModel.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace Sales.DataModel
{
    public class Admin : IdentityUser
    {

        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<Admin> manager)
        {
            // Note the authenticationType must match the one 
            // defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity =
                await manager.CreateIdentityAsync(this,
                    DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
