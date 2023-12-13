using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        private readonly MVCAppDbContext _dbContext;
        public UnitOfWork(MVCAppDbContext dbContext) // ASK CLR For Object 
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
         =>await _dbContext.SaveChangesAsync();

        public void Dispose()
        => _dbContext.Dispose();
    }
}
