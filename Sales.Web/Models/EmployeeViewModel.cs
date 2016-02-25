using Sales.DataModel.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales.Web.Models
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        public string FirstName {get; set;}
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyId { get; set; }
        public List<Button> BtnList { get; set; }
    }
}