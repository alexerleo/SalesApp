using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales.DataServices.ServiceModels
{
    public class CompaniesDataTableContainer
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<CompanyDTO> data { get; set; }
    }
}