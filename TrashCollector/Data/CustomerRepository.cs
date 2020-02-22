using Microsoft.EntityFrameworkCore;
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
        public Customer GetCustomer(int id) => FindByCondition(c => c.Id == id).Include(c=> c.Address).Include(c => c.Pickup).SingleOrDefault();
        public Customer GetCustomer(string userId) => FindByCondition(c => c.UserId == userId).Include(c => c.Address).Include(c => c.Pickup).SingleOrDefault();
        public IQueryable<Customer> GetCustomersByZipCode(int zipCode) => FindByCondition(c => c.Address.ZipCode == zipCode).Include(c => c.Address).Include(c => c.Pickup);
        public IQueryable<Customer> GetCustomersByZipCodeAndPickupDay(int zipCode, string day) => FindByConditionWithInclude(c => c.Address.ZipCode == zipCode && c.Pickup.PickupDay == day, a => a.Address, p => p.Pickup);
        public IQueryable<Customer> FilterCustomersByPickupDay(string day) => FindByCondition(c => c.Pickup.PickupDay == day);
        public IQueryable<Customer> GetCustomers() => FindAll().Include(c => c.Address).Include(c => c.Pickup);

    }
}
