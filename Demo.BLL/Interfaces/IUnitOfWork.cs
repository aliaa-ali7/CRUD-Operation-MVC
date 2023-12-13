using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        // Signatures for properties for every and each repository

        public IEmployeeRepository EmployeeRepository { get; set; }

        public IDepartmentRepository DepartmentRepository { get; set; }

        Task<int> CompleteAsync();

    }
}
