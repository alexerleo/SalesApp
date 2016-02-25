using Microsoft.AspNet.Identity;
using Sales.DataModel.Companies;
using Sales.DataModel.DbConfig;
using Sales.DataModel.Mobile;
using Sales.DataServices.ServiceModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum StatusCode
{
    Success = 0, MissingRequiredFields, WrongLoginOrPass, InvalidSessionToken, WrongId, UnknownError
}
namespace Sales.DataServices.ServiceClases
{
    class GroupInfo
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
    }
    public class MobileService : SalesBaseService
    {
        public MobileService(SalesDbContext context) : base(context)
        { 
            
        }
        public LoginResponse Login(string email, string password)
        {
            LoginResponse response = new LoginResponse();
            if(email != null && password != null)
            {
                Employee employee = _db.Employees.FirstOrDefault(x => (x.Email == email && x.Password == password));
                if (employee != null)
                {
                    SessionToken token = new SessionToken();
                    token.EmployeeId = employee.Id;
                    response.companyName = _db.Companies.Find(employee.CompanyId).Name;
                    response.btnList = ConvertButtonsToDTO(_db.Buttons.Where( b => b.CompanyId == employee.Company.Id).ToList());
                    response.email = employee.Email;
                    response.status = response.status = new Status() { message = "ok", statusCode = ((int)StatusCode.Success).ToString() };
                    response.sessionToken = token.Id.ToString();
                    _db.SessionTokens.Add(token);
                    _db.SaveChanges();
                }
                else
                {
                    response.status = new Status() { message = "wrongLoginOrPass", statusCode = ((int)StatusCode.WrongLoginOrPass).ToString() };
                }
            }
            else
            {
                response.status = new Status() { message = "missingRequiredFields" , statusCode =((int) StatusCode.MissingRequiredFields).ToString() };
            }
            
            return response;

        }
        public object ClickOnButton(Guid sessionTokenId, Guid btnId, double latitude, double longitude)
        {
            var sessionToken = _db.SessionTokens.FirstOrDefault(s => s.Id == sessionTokenId);
            if ( sessionToken != null)
            { 
                //delete previous session token of it existed
                var employee = _db.Employees.Find(sessionToken.EmployeeId);
                var button = _db.Buttons.Find(btnId);
                var click = new Click() {
                    Button = button,
                    ButtonId = button.Id,
                    Date = DateTime.Now,
                    Latitude = latitude,
                    Longitude = longitude,
                    Id = Guid.NewGuid(),
                    EmployeeId = employee.Id,
                    Employee = employee
                };
                _db.Clicks.Add(click);
                _db.SaveChanges();
                return new { status = new Status() { message = "ok", statusCode = ((int)StatusCode.Success).ToString() } };
            }
            else 
            {
                return new { status = new Status() { message = "invalidSessionToken", statusCode = ((int)StatusCode.InvalidSessionToken).ToString() } };
            }
        }
        public BtnStatisticResponce BtnStatistic (Guid sessionTokenId)
        {
            BtnStatisticResponce responce = new BtnStatisticResponce();
            var sessionToken = _db.SessionTokens.FirstOrDefault(s => s.Id == sessionTokenId);
            if (sessionToken != null)
            {
                var employee = _db.Employees.Find(sessionToken.EmployeeId);
                responce.BtnList = ConvertButtonsToBtnStatisticListItem(employee.CompanyId, employee.Id);
                responce.status = new Status() { message = "ok", statusCode = ((int)StatusCode.Success).ToString() };
            }
            else
            {
                responce.status = new Status() { message = "invalidSessionToken", statusCode = ((int)StatusCode.InvalidSessionToken).ToString() };
            }
            return responce;
        }
        private List<BtnStatisticListItem> ConvertButtonsToBtnStatisticListItem(Guid CompId, Guid empId)//refactor
        {
            List<BtnStatisticListItem> resultList = new List<BtnStatisticListItem>();
            var empDataMonth = (from u in _db.Clicks
                             where u.EmployeeId == empId
                             where u.Date > DbFunctions.AddDays(DateTime.Now, -30)
                             group u by u.ButtonId into clickGroup
                             let count = clickGroup.Count()
                             orderby count descending
                             select new GroupInfo
                             {
                                 Id = clickGroup.FirstOrDefault().ButtonId,
                                 Count = count
                             }).ToList();
            var empDataWeek = (from u in _db.Clicks
                             where u.EmployeeId == empId
                             where u.Date > DbFunctions.AddDays(DateTime.Now, -7)
                             group u by u.ButtonId into clickGroup
                             let count = clickGroup.Count()
                             orderby count descending
                             select new GroupInfo
                             {
                                 Id = clickGroup.FirstOrDefault().ButtonId,
                                 Count = count
                             }).ToList();
            var empDataDay = (from u in _db.Clicks
                            where u.EmployeeId == empId
                            where DbFunctions.TruncateTime(u.Date) == DbFunctions.TruncateTime(DateTime.Now)
                            group u by u.ButtonId into clickGroup
                            let count = clickGroup.Count()
                            orderby count descending
                            select new GroupInfo
                            {
                                Id = clickGroup.FirstOrDefault().ButtonId,
                                Count = count
                            }).ToList();

            var allButtonDataMonth = (from u in _db.Clicks
                            where u.Employee.CompanyId == CompId
                            where u.Date > DbFunctions.AddDays(DateTime.Now, -30)
                            group u by u.ButtonId into clickGroup
                            let count = clickGroup.Count()
                            orderby count descending
                            select new GroupInfo
                            {
                                Id = clickGroup.FirstOrDefault().ButtonId,
                                Count = count
                            }).ToList();
            var allButtonDataWeek = (from u in _db.Clicks
                                where u.Employee.CompanyId == CompId
                                where u.Date > DbFunctions.AddDays(DateTime.Now, -7)
                                group u by u.ButtonId into clickGroup
                                let count = clickGroup.Count()
                                orderby count descending
                                select new GroupInfo
                                {
                                    Id = clickGroup.FirstOrDefault().ButtonId,
                                    Count = count
                                }).ToList();
            var allButtonDataDay = ( from u in _db.Clicks
                                     where u.Employee.CompanyId == CompId
                                     where DbFunctions.TruncateTime(u.Date) == DbFunctions.TruncateTime(DateTime.Now)
                                     group u by u.ButtonId into clickGroup
                                     let count = clickGroup.Count()
                                     orderby count descending
                                     select new GroupInfo
                                     {
                                         Id = clickGroup.FirstOrDefault().ButtonId,
                                         Count = count
                                     }).ToList();

            var allButtons = _db.Buttons.Where(b => b.CompanyId == CompId).OrderBy(b=> b.OrderIndex).ToList();
            foreach (var item in allButtons)
            {
                resultList.Add(new BtnStatisticListItem()
                {
                    id = item.Id,
                    text = item.Text,
                    textColor = item.FontColor,
                    highestClicksMonth = allButtonDataMonth.Count > 0 ? (allButtonDataMonth.Where(g => g.Id == item.Id).FirstOrDefault() != null ? allButtonDataMonth.Where(g => g.Id == item.Id).FirstOrDefault().Count.ToString() : "0") : "0",
                    highestClicksWeek = allButtonDataWeek.Count > 0 ? (allButtonDataWeek.Where(g => g.Id == item.Id).FirstOrDefault() != null ? allButtonDataWeek.Where(g => g.Id == item.Id).FirstOrDefault().Count.ToString() : "0") : "0",
                    highestClicksToday = allButtonDataDay.Count > 0 ? (allButtonDataDay.Where(g => g.Id == item.Id).FirstOrDefault() != null ? allButtonDataDay.Where(g => g.Id == item.Id).FirstOrDefault().Count.ToString() : "0") : "0",
                    userClicksMonth = empDataMonth.Count > 0 ? (empDataMonth.Where(g => g.Id == item.Id).FirstOrDefault() != null ? empDataMonth.Where(g => g.Id == item.Id).FirstOrDefault().Count.ToString() : "0") : "0",
                    userClicksWeek = empDataWeek.Count > 0 ? (empDataWeek.Where(g => g.Id == item.Id).FirstOrDefault() != null ? empDataWeek.Where(g => g.Id == item.Id).FirstOrDefault().Count.ToString() : "0") : "0",
                    userClicksToday = empDataDay.Count > 0 ? (empDataDay.Where(g => g.Id == item.Id).FirstOrDefault() != null ? empDataDay.Where(g => g.Id == item.Id).FirstOrDefault().Count.ToString() : "0") : "0",
                });
            }
            return resultList;
        }
        private List<ButtonDTO> ConvertButtonsToDTO(List<Button> list)
        {
            List<ButtonDTO> resultList = new List<ButtonDTO>();
            foreach(var item in list)
            {
                resultList.Add(new ButtonDTO(){
                    color = item.FontColor,
                    bgColor = item.BgColor,
                    id = item.Id.ToString(),
                    text = item.Text
                });
            }
            return resultList;
        }

    }
}
