using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceModels
{
    public class BtnStatisticResponce
    {
        public BtnStatisticResponce()
        {
            BtnList = new List<BtnStatisticListItem>();
        }
        public List<BtnStatisticListItem> BtnList { get; set; }
        public Status status { get; set;}
    }
    public class BtnStatisticListItem
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public string textColor { get; set; }
        public string userClicksToday { get; set; }
        public string highestClicksToday { get; set; }
        public string userClicksWeek { get; set;}
        public string highestClicksWeek { get; set; }
        public string userClicksMonth { get; set; }
        public string highestClicksMonth { get; set; }
    }
}
