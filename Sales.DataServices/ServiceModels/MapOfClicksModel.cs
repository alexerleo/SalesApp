using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceModels
{
    class MapOfClicksModel
    {
        public Guid CompanyId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? ButtonId { get; set; }
        public string Id { get; set; }
    }
}
