using Sales.DataModel.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataModel.Mobile
{
    public class SessionToken
    {
        public SessionToken()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id {get; set;}
        public Guid EmployeeId { get; set;}
        public Employee Employee { get; set; }
    }
}
