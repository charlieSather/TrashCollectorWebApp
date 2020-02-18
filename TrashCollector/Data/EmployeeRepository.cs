using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Contracts;
using TrashCollector.Models;

namespace TrashCollector.Data
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
        }

        public Employee GetEmployee(int id) => FindByCondition(e => e.Id == id).SingleOrDefault();
        public Employee GetEmployee(string userId) => FindByCondition(e => e.UserId == userId).SingleOrDefault();
        public void CreateEmployee(Employee employee) => Create(employee);


    }
}
