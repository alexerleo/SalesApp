using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataModel.Companies
{
    public class Company
    {
        public Company()
        {
            Buttons = new List<Button>();
            Employees = new LinkedList<Employee>();
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool ActivityStatus { get; set; }
        public Guid Id { get; set; }
        public ICollection<Button> Buttons { get; set; }
        public ICollection<Admin> Admins { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
