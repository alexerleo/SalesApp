using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataModel.Companies
{
    public class Button
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string FontColor { get; set; }
        public string BgColor { get; set; }
        public int OrderIndex { get; set;}
        public ICollection<Click> Clicks { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
