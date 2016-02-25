using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceModels
{
    public class CompanyTableDTO
    {
        public string CompanyName { get; set; }
        public CompanyTableDTO()
        {
            buttonListNames = new List<string>();
            Items = new List<CompanyItem>();
        }
        public List<string> buttonListNames { get; set; }
        public List<CompanyItem> Items { get; set; }
    }
    public class CompanyItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> buttonList { get; set; }
    }
}
