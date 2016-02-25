using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataModel.Companies
{
    public class Click
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Date { get; set; }
        public Guid ButtonId { get; set; }
        public Button Button {get; set;}
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
