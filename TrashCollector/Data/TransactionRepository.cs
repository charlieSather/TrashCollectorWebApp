using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Contracts;
using TrashCollector.Models;

namespace TrashCollector.Data
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
        }
        public IQueryable<Transaction> GetTransactions() => FindAll()
            .Include(t => t.Customer).ThenInclude(c => c.Address).Include(t => t.Customer).ThenInclude(c => c.Pickup).Include(t => t.Employee);
        public IQueryable<Customer> GetCustomers(int zipCode,string day) => 
            FindByCondition(t => t.Customer.Address.ZipCode.Equals(zipCode) && t.Customer.Pickup.PickupDay.Equals(day))
                .Include(t => t.Customer).ThenInclude(c => c.Address).Include(t => t.Customer).ThenInclude(c => c.Pickup).Include(t => t.Employee)
                .Select(x => x.Customer);
        public void CreateTransaction(Transaction transaction) => Create(transaction);

        public IQueryable<Transaction> GetCustomersTransactionsThisMonth(int customerId) =>
            FindByCondition(t => t.CustomerId == customerId && t.ChargeDate.Month == DateTime.Today.Month && t.ChargeDate.Year == DateTime.Today.Year)
                .Include(t => t.Customer).ThenInclude(c => c.Address).Include(t => t.Customer).ThenInclude(c => c.Pickup).Include(t => t.Employee);

        public IQueryable<Transaction> GetTransactionsToday(DateTime today) =>
            FindByCondition(t => t.ChargeDate.Date == today.Date)
                .Include(t => t.Customer).ThenInclude(c => c.Address).Include(t => t.Customer).ThenInclude(c => c.Pickup).Include(t => t.Employee);

    }
}
