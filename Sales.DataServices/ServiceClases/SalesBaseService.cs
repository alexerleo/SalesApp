using Sales.DataModel.DbConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceClases
{
    public class SalesBaseService 
    {
        protected SalesDbContext _db;
        public SalesBaseService(SalesDbContext context)
        {
            _db = context;
        }
        
    }
}
