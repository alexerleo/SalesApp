using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sales.DataServices.ServiceModels;

namespace Sales.Web.Models
{
    public class MapViewModel
    {
        public Guid CompanyId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? BtnId { get; set; }
        public List<PointDTO> Points { get; set; }
    }
}