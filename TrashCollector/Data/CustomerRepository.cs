using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Contracts;
using TrashCollector.Models;

namespace TrashCollector.Data
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
        }

        public void CreateCustomer(Customer customer) => Create(customer);
        public Customer GetCustomer(int id) => FindByCondition(c => c.Id == id).SingleOrDefault();
        public Customer GetCustomer(string userId) => FindByCondition(c => c.UserId == userId).SingleOrDefault();
        public IQueryable<Customer> GetCustomersByZipCode(int zipCode) => FindByConditionWithInclude(c => c.Address.ZipCode == zipCode,a => a.Address , p => p.Pickup);
        public IQueryable<Customer> FilterCustomersByPickupDay(string day) => FindByCondition(c => c.Pickup.PickupDay == day);

    }
}
