using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _dbContext;
        public EmployeeRepository(MVCAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
       
         =>   _dbContext.Employees.Where(E => E.Address == address);

       
        IQueryable<Employee> IEmployeeRepository.SearchEmployeesByName(string SearchValue)
        {
            return _dbContext.Employees.Where(E=> E.Name.ToLower().Contains(SearchValue.ToLower()));
        }
    }
}
