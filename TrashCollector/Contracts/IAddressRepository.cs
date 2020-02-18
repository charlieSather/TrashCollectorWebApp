using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Models;

namespace TrashCollector.Contracts
{
    public interface IAddressRepository : IRepositoryBase<Address>
    {
        Address GetAddress(int id);
        int GetAddressId(Address address);
        void CreateAddress(Address address);
        bool AddressExists(Address address);

    }
}
