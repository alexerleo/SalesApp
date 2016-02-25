using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales.DataServices.ServiceModels
{
    public class CompanyDTO
    {
        public string Name { get; set; }
        public string CompanyId { get; set; }
        public string Mobile { get; set; }
        public string NumberOfEmploees { get; set; }
        public string ActivityStatus { get; set; }
    }
}