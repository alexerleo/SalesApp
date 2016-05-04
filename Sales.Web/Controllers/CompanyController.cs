using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sales.DataModel;
using Sales.DataModel.Companies;
using Sales.DataModel.DbConfig;
using Sales.DataServices.ServiceClases;
using Sales.DataServices.ServiceModels;
using Sales.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sales.Web.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private UserManager<Admin> _userManager;
        private CompanyService _companyService;
        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
            _userManager = new UserManager<Admin>(new UserStore<Admin>(new SalesDbContext()));
            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 2,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

        }
        public ActionResult IndexPage(Guid id)
        {
            ViewBag.CompanyId = id;
            return View(_companyService.GetCompanyTable(id));
        }
        public ActionResult ButtonsGraphic()
        {

            return View();
        }
        public ActionResult AddButton(Guid companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }
        [HttpPost]
        public ActionResult AddButton(AddButtonModel model)
        {
            if (!(string.IsNullOrEmpty(model.Text)))
            {
                _companyService.AddButton(model.CompanyId, new Button()
                {
                    BgColor = model.BgColor,
                    FontColor = model.FontColor,
                    Text = model.Text
                });
                return RedirectToAction("IndexPage", new { id = model.CompanyId });
            }
            else
            {
                ModelState.AddModelError("", "Button text is required");
                return RedirectToAction("AddButton", new { id = model.CompanyId });
            }
            
        }
        public ActionResult Buttons(Guid companyId)
        {
            ViewBag.CompanyId = companyId;
            return View(_companyService.GetAllButtons(companyId));
        }
        public ActionResult AddEmployee(Guid companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(AddEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                _companyService.AddEmploee(model.CompanyId, new Employee() { 
                    Email = model.Email, 
                    FirstName = model.Name, 
                    LastName = model.LastName,
                    Password = model.Password,
                    Id = Guid.NewGuid()
                
                });
                return RedirectToAction("IndexPage", new { id = model.CompanyId });
            }
            else
                return RedirectToAction("AddEmployee", new { id = model.CompanyId });
            
        }

        public ActionResult AddAdmin(Guid companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.MainAdmin + ", " + RoleNames.Admin)]
        public async Task<ActionResult> AddAdmin(AddAdminViewModel model)
        {
            ViewBag.CompanyId = model.CompanyId;
            if (ModelState.IsValid)
            {
                    var admin = new Admin() { UserName = model.AdminName, CompanyId = model.CompanyId };
                    var result = await _userManager.CreateAsync(admin, model.Password);
                    if (result.Succeeded)
                    {
                        _userManager.AddToRole(admin.Id, RoleNames.Admin);
                        return RedirectToAction("IndexPage", new { id = model.CompanyId });
                    }
                    else
                    {
                        AddErrors(result);
                        return View();
                    }
                }
                else {
                    ModelState.AddModelError("", "Missing required fields");
                    return View();
                }
        }
        public ActionResult EmployeeGraphic(Guid EmployeeId, DateTime? from = null, DateTime? to = null)
        {            
            var data = _companyService.EmpGraph(EmployeeId, from, to);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeeAccount(Guid EmployeeId, DateTime? From = null, DateTime? To = null)
        {
            var employee = _companyService.GetEmployee(EmployeeId);
            EmployeeViewModel emploeeVM = new EmployeeViewModel() { 
                CompanyId = employee.CompanyId.ToString(), 
                Email = employee.Email, 
                FirstName = employee.FirstName, 
                LastName = employee.LastName, 
                Password = employee.Password, 
                Id = employee.Id.ToString(), 
                BtnList = _companyService.FilterButtonClicks(employee.CompanyId, EmployeeId, From, To)
            };
            ViewBag.From = From;
            ViewBag.To = To;
            return View(emploeeVM);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public string UpdateEmployee(Guid id, string value, int? rowId, int? columnPosition, int columnId, string columnName)
        {
            _companyService.UpdateEmployee(id, columnId, value);
            return value;
        }
        public ActionResult DeleteButton(Guid buttonId, Guid companyId)
        {
            _companyService.DeleteButton(buttonId);
            return RedirectToAction("Buttons", new { companyId = companyId });
        }
        public string UpdateButton(Guid id, string value, int columnId)
        {
            if(columnId != 3)
                value = "#" + value;
            _companyService.UpdateButton(id,columnId,value);
            return value;
        }
        public ActionResult UpdateOrder(Guid id, int fromPosition, int toPosition, string direction)
        {
            _companyService.UpdateButtonsOrder(id, fromPosition, toPosition, direction);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public ActionResult MapOfClicks(Guid companyId, Guid? employeeId, DateTime? from, DateTime? to, Guid? btnId)
        {            
            var mapVM = new MapViewModel()
            {
                CompanyId = companyId,
                EmployeeId = employeeId,
                From = from,
                To = to,
                BtnId = btnId,
                Points = _companyService.GetPoints(companyId, employeeId, from, to, btnId) as List<PointDTO>
            };
            return View(mapVM);
        }
        public ActionResult EmployeeClicksMap(Guid companyId, Guid? employeeId, DateTime? from, DateTime? to, Guid? btnId)
        {
            var data = _companyService.GetPoints(companyId, employeeId, from, to, btnId) as List<PointDTO>;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [ChildActionOnly]
        public ActionResult CompanyAdminLinksPartial()
        {
            Guid id = new Guid( User.Identity.GetUserId());
            Guid? companyId = _companyService.GetCompanyByAdminId(id);
            return PartialView(companyId);
        }
	}
}