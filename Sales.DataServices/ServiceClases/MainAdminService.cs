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
    public class MainAdminService : SalesBaseService
    {
        public MainAdminService(SalesDbContext context) : base(context)
        { 
            
        }
        public void AddCompany(Company c)
        {
            c.Id = Guid.NewGuid();
            _db.Companies.Add(c);
            _db.SaveChanges();
        }
        public void DeleteCompany(Guid id)
        {
            var company = _db.Companies.Find(id);
            company.ActivityStatus = false;
            _db.Entry(company ).State = EntityState.Modified;
            _db.SaveChanges();
        }
        public void RestoreCompany(Guid id)
        {
            var company = _db.Companies.Find(id);
            company.ActivityStatus = true;
            _db.Entry(company).State = EntityState.Modified;
            _db.SaveChanges();
        }
        public CompaniesDataTableContainer GetFilteredCompanies(int _draw, int start, int length, string search)
        {
            IQueryable<Company> qry;
            if(string.IsNullOrEmpty(search))
            {
                qry = _db.Companies;
            }
            else
            {
                qry = _db.Companies.Where(c => c.Name.Contains(search));
            }
            int totalUsersCount = qry.Count(x => true);
            var resultList = ConvertCompanyListToDTO(qry.OrderBy(c => c.Name).Skip(start).Take(length).ToList());
            return new CompaniesDataTableContainer() { draw = _draw, recordsFiltered = totalUsersCount, recordsTotal = totalUsersCount, data = resultList };
        }
        private List<CompanyDTO> ConvertCompanyListToDTO(List <Company> companies)
        {
            int numOfEmploees;
            List<CompanyDTO> result = new List<CompanyDTO>();
            foreach (var item in companies)
            {
                numOfEmploees = _db.Employees.Where(x => x.CompanyId == item.Id).Count(x => true);
                result.Add(new CompanyDTO()
                {
                    ActivityStatus = item.ActivityStatus.ToString(),
                    CompanyId = item.Id.ToString(),
                    Mobile = item.Mobile,
                    Name = item.Name,
                    NumberOfEmploees = numOfEmploees.ToString()
                });

            }
            return result;
        }
    }
}
