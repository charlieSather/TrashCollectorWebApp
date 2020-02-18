﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Models;

namespace TrashCollector.Contracts
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Customer GetCustomer(int id);
        Customer GetCustomer(string userId);
        void CreateCustomer(Customer customer);

        IQueryable<Customer> GetCustomersByZipCode(int zipCode);
        //Pickup GetPickup(int id);
        //Address GetAddress(int id);


    }
}
