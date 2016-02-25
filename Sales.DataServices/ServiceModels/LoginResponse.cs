using Sales.DataModel.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceModels
{
    public class LoginResponse
    {
        public string companyName { get; set; }
        public string email { get; set; }
        public List<ButtonDTO> btnList { get; set; }
        public string sessionToken { get; set; }
        public Status status { get; set; }
    }

    public class ButtonDTO
    {
        public string id { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string bgColor { get; set; }
    }
}