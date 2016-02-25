using Sales.DataModel.DbConfig;
using Sales.DataModel.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataModel.Companies
{
    public class Employee
    {

        public Employee()
        {
            Clicks = new List<Click>();
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Click> Clicks { get; set; }
        public ICollection<SessionToken> SessionTokens { get; set; }
    }
}
