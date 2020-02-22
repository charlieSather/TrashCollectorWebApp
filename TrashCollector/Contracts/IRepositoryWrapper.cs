using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Contracts
{
    public interface IRepositoryWrapper
    {
        ICustomerRepository Customer { get; }
        IEmployeeRepository Employee { get; }
        IAddressRepository Address { get; }
        IPickupRepository Pickup { get; }
        ITransactionRepository Transaction { get; }
        void Save();
    }
}
