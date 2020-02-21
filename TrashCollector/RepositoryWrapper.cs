using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Contracts;
using TrashCollector.Data;


namespace TrashCollector
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _context;
        private ICustomerRepository _customer;
        private IEmployeeRepository _employee;
        private IAddressRepository _address;
        private IPickupRepository _pickup;
        private ITransactionRepository _transaction;


        public ICustomerRepository Customer
        {
            get
            {
                if (_customer is null)
                {
                    _customer = new CustomerRepository(_context);
                }
                return _customer;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if(_employee is null)
                {
                    _employee = new EmployeeRepository(_context);
                }
                return _employee;
            }
        }

        public IAddressRepository Address
        {
            get
            {
                if(_address is null)
                {
                    _address = new AddressRepository(_context);
                }
                return _address;
            }
        }
        
        public IPickupRepository Pickup
        {
            get
            {
                if(_pickup is null)
                {
                    _pickup = new PickupRepository(_context);
                }
                return _pickup;
            }
        }
        
        public ITransactionRepository Transaction
        {
            get
            {
                if(_transaction is null)
                {
                    _transaction = new TransactionRepository(_context);
                }
                return _transaction;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
