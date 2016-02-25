using Sales.DataModel.DbConfig;
using Sales.DataServices.ServiceClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales.Web.Controllers
{
    public class MobileController : Controller
    {
        MobileService service;
        public MobileController()
        {
            service = new MobileService(new SalesDbContext());
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            return Json(service.Login(email, password), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Click(Guid sessionToken, Guid btnId, string latitude, string longitude)
        {
            double latitudeD = Convert.ToDouble(latitude);
            double longitudeD = Convert.ToDouble(longitude);
            return Json( service.ClickOnButton(sessionToken, btnId, latitudeD, longitudeD), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ButtonStatistic(Guid sessionToken)
        {
            return Json(service.BtnStatistic(sessionToken) ,JsonRequestBehavior.AllowGet);
        }
	}
}