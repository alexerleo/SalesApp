using Sales.DataModel;
using Sales.DataModel.Companies;
using Sales.DataModel.DbConfig;
using Sales.DataServices.ServiceModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.DataServices.ServiceClases
{
    public class CompanyService : SalesBaseService
    {
        public CompanyService(SalesDbContext context)
            : base(context)
        { 
            
        }
        public Company GetCompany(Guid id)
        {
            return _db.Companies.Find(id);
        }
        public void  AddEmploee(Guid companyId, Employee emp)
        {
            emp.Id = Guid.NewGuid();
            emp.Company = _db.Companies.Find(companyId);
            emp.CompanyId = companyId;
            _db.Employees.Add(emp);
            _db.SaveChanges();
        }
        public void AddAdmin(Guid companyId, Admin admin)
        {
            admin.Company = _db.Companies.Find(companyId);
            _db.Users.Add(admin);
            _db.SaveChanges();
        }
        public void AddButton(Guid companyId, Button button)
        {
            button.Company = _db.Companies.Find(companyId);
            button.Id = Guid.NewGuid();
            var companyButtons = _db.Buttons.Where(b => b.CompanyId == companyId);            
            int index = companyButtons.Count() != 0 ? companyButtons.Max(b => b.OrderIndex) : 0;
            button.OrderIndex = index + 1;
            _db.Buttons.Add(button);
            _db.SaveChanges();
        }
        public List<Button> GetAllButtons(Guid companyId, Guid? employeeId = null)
        {
           return _db.Buttons
                .Where(x => x.CompanyId == companyId)
                .OrderBy(b => b.OrderIndex)
                .ToList()
                .Select(x => new Button()
                {
                    BgColor = x.BgColor,
                    Company = x.Company,
                    CompanyId = x.CompanyId,
                    FontColor = x.FontColor,
                    Id = x.Id,
                    OrderIndex = x.OrderIndex,
                    Text = x.Text,
                    Clicks = employeeId.HasValue
                        ? _db.Clicks.Where(click => click.EmployeeId == employeeId.Value && click.ButtonId == x.Id).ToList()
                        : new List<Click>()
                })
                .ToList();
        }
        public List<Button> FilterButtonClicks(Guid companyId, Guid employeeId, DateTime from, DateTime to)
        {
            return _db.Buttons
                .Where(x => x.CompanyId == companyId)
                .OrderBy(b => b.OrderIndex)
                .ToList()
                .Select(x => new Button()
                {
                    BgColor = x.BgColor,
                    Company = x.Company,
                    CompanyId = x.CompanyId,
                    FontColor = x.FontColor,
                    Id = x.Id,
                    OrderIndex = x.OrderIndex,
                    Text = x.Text,
                    Clicks = _db.Clicks.Where(click => click.EmployeeId == employeeId 
                        && click.ButtonId == x.Id 
                        && click.Date < to 
                        && click.Date >= from)
                        .ToList()
                })
                .ToList();           
        }
        public CompanyTableDTO GetCompanyTable(Guid companyId)
        {
            CompanyTableDTO result = new CompanyTableDTO();
            var employeesList = _db.Employees.Where(e => e.CompanyId == companyId);
            var clicksMonth = (from u in _db.Clicks
                             where u.Employee.CompanyId == companyId
                             where u.Date > DbFunctions.AddDays(DateTime.Now, -30)
                             group u by u.ButtonId).ToList();
            var companyButtons = _db.Buttons.Where( b => b.CompanyId == companyId);
            foreach(var item in companyButtons)
            {
                result.buttonListNames.Add(item.Text);
            }
            List<string> btnList = new List<string> ();
            IGrouping<Guid, Click> clickGroup;
            foreach( var item in employeesList)
            {
                foreach (var button in companyButtons)
                {
                    clickGroup = clicksMonth.FirstOrDefault(g => g.FirstOrDefault().ButtonId == button.Id);
                    if (clickGroup != null)
                    {
                        btnList.Add(clickGroup.Where(c => c.EmployeeId == item.Id).ToList().Count.ToString());
                    }
                    else
                        btnList.Add("0");
                }
                result.Items.Add(new CompanyItem()
                {
                    Id = item.Id,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Password = item.Password,
                    buttonList = btnList
                });
                btnList = new List<string>();
            }
            result.CompanyName = _db.Companies.Find(companyId).Name;
            return result;
        }

        public void UpdateEmployee(Guid id, int columnId, string value)
        {
            var emp = _db.Employees.Find(id);
            switch (columnId)
            { 
                case 0:
                    emp.FirstName = value;
                    break;
                case 1:
                    emp.LastName = value;
                    break;
                case 2:
                    emp.Email = value;
                    break;
                case 3:
                    emp.Password = value;
                    break;
            }
            _db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public Employee GetEmployee(Guid id)
        {
            return _db.Employees.Find(id);
        }
        public void DeleteButton(Guid id)
        {
            _db.Buttons.Remove(_db.Buttons.Find(id));
            _db.SaveChanges();
        }

        public void UpdateButton(Guid id, int columnId, string value)
        {
            var btn = _db.Buttons.Find(id);
            switch (columnId)
            {
                case 1:
                    btn.BgColor = value;
                    break;
                case 2:
                    btn.FontColor = value;
                    break;
                case 3:
                    btn.Text = value;
                    break;
            }
            _db.Entry(btn).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public void UpdateButtonsOrder(Guid id, int fromPosition, int toPosition, string direction)
        {
            List<Button> movedButtons;
            if (direction == "back")
            {
                movedButtons = _db.Buttons
                            .Where(b => (toPosition <= b.OrderIndex && b.OrderIndex <= fromPosition))
                            .ToList();
                foreach (var item in movedButtons)
                {
                    item.OrderIndex++;
                }
            }
            else
            {
                movedButtons = _db.Buttons
                            .Where(b => (fromPosition <= b.OrderIndex && b.OrderIndex <= toPosition))
                            .ToList();
                foreach (var item in movedButtons)
                {
                    item.OrderIndex--;
                }
            }

            _db.Buttons.First(c => c.Id == id).OrderIndex = toPosition;
            foreach (var item in movedButtons)
            {
                _db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            _db.SaveChanges();
        }
        public object EmpGraph(Guid empoyeeId, DateTime? From = null, DateTime? To = null)
        {
            var employee = _db.Employees.Find(empoyeeId);
            List<BtnStatisticListItem> resultList = new List<BtnStatisticListItem>();
            var empDataMonth = (from u in _db.Clicks
                                where u.EmployeeId == empoyeeId
                                    && u.Date > (From ?? DbFunctions.AddDays(DateTime.Now, -30))
                                    && u.Date < (To ?? DateTime.Now)
                                group u by u.ButtonId into clickGroup
                                let count = clickGroup.Count()
                                orderby count descending
                                select new 
                                {
                                    ButtonId = clickGroup.FirstOrDefault().ButtonId,
                                    Count = count
                                }).ToList();
            var allButtons = _db.Buttons.Where(b => b.CompanyId == employee.CompanyId);
            var buttonNames = new List<string>();
            var buttonClicksCount = new List<int>();
            foreach (var button in allButtons)
            {
                buttonNames.Add(button.Text);
                if (empDataMonth.Where(o => o.ButtonId == button.Id).FirstOrDefault() != null)
                {
                    buttonClicksCount.Add(empDataMonth.Where(o => o.ButtonId == button.Id).FirstOrDefault().Count);
                }
                else
                {
                    buttonClicksCount.Add(0);
                }
            }
            return new { ButtonNames = buttonNames.ToArray(), buttonClicksCount = buttonClicksCount.ToArray()};
        }
        public object GetPoints(Guid companyId, Guid? empoyeeId, DateTime? from, DateTime? to, Guid? buttonId)
        {
            if (empoyeeId == null)
            {
                return ConvertClicksToPoints(_db.Clicks.Include("Employee").Include("Button").Where(c => c.Employee.CompanyId == companyId).ToList());
            }
            else 
            {
                var result = _db.Clicks.Include("Employee").Include("Button").Where(c => c.EmployeeId == empoyeeId);
                if (buttonId != null)
                {
                    result = result.Where(c => c.ButtonId == buttonId);
                }
                if (from.HasValue && to.HasValue)
                {
                    result = result.Where(c => (c.Date > from && c.Date < to));
                }
                return ConvertClicksToPoints(result.ToList());
            }
        }
        public Guid? GetCompanyByAdminId(Guid adminId)
        {
            return _db.Users.Find(adminId.ToString()).CompanyId;
        }
        #region/privat/
        private List<PointDTO> ConvertClicksToPoints(List<Click> clicks)
        {
            var result = new List<PointDTO>();
            foreach (var item in clicks)
            { 
                result.Add(new PointDTO(){
                   EmployeeName = item.Employee.FirstName,
                   EmployeeLastName = item.Employee.LastName,
                   Time = item.Date.ToString(),
                   ButtonText = item.Button.Text,
                   Latitude = item.Latitude.ToString(),
                   Longitude = item.Longitude.ToString()
                });
            }
            return result;
        }
        #endregion
    }
    
}
