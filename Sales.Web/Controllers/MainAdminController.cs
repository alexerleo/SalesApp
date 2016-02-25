using Sales.DataModel;
using Sales.DataModel.Companies;
using Sales.DataServices.ServiceClases;
using Sales.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sales.Web.Controllers
{
    [Authorize(Roles = RoleNames.MainAdmin)]
    public class MainAdminController : Controller
    {
        private readonly MainAdminService _service;
        public MainAdminController(MainAdminService service)
        {
            _service = service;
        }
        public ActionResult Companies()
        {
            return View();
        }
        public ActionResult AddCompany()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCompany(AddCompanyViewModel model)
        {
            if (model.CompanyName != null)
            {
                if (ValidateEmail(model.Email))
                {
                    var c = new Company() { Name = model.CompanyName, Email = model.Email, Mobile = model.Mobile, ActivityStatus = true };
                    _service.AddCompany(c);
                    return RedirectToAction("Companies");
                }
                else
                {
                    ModelState.AddModelError("", "Email is incorrect");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Name is required field");
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult DeleteCompany(string id)
        {
            _service.DeleteCompany(new Guid(id));
            return View();
        }
        [HttpPost]
        public ActionResult RestoreCompany(string id)
        {
            _service.RestoreCompany(new Guid(id));
            return View();
        }
        public ActionResult GetCompaniesTable(int draw, int start, int length)
        {
            string search = Request.QueryString["search[value]"];
            return Json(_service.GetFilteredCompanies(draw, start, length,search), JsonRequestBehavior.AllowGet);
        }
        private bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
	}
}
