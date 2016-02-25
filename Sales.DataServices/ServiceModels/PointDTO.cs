using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceModels
{
    public class PointDTO
    {
        public string Time { get; set; }
        public string EmployeeName { get; set; }
        public string ButtonText { get; set; }
        public string EmployeeLastName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
