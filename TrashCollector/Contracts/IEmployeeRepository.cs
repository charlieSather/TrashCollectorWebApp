using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Models;

namespace TrashCollector.Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Employee GetEmployee(int id);
        Employee GetEmployee(string userId);
        void CreateEmployee(Employee employee);
    }
}
