using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Models;
namespace TrashCollector.Contracts
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        IQueryable<Transaction> GetTransactions();
        IQueryable<Transaction> GetTransactionsToday(DateTime today);
        IQueryable<Transaction> GetCustomersTransactionsThisMonth(int customerId);
        IQueryable<Transaction> GetCustomersTransactions(int customerId);
        IQueryable<Customer> GetCustomers(int zipCode, string day);
        IQueryable<Transaction> GetEmployeeTransactionsToday(int employeeId);
        void CreateTransaction(Transaction transaction);
    }
}
