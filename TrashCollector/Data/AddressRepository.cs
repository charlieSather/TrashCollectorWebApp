using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Contracts;
using TrashCollector.Models;

namespace TrashCollector.Data
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
        }

        public void CreateAddress(Address address) => Create(address);

        public Address GetAddress(int id) => FindByCondition(a => a.Id == id).SingleOrDefault();
        public bool AddressExists(Address address) => FindByCondition(a => a.StreetAddress.Equals(address.StreetAddress) && a.State.Equals(address.State) && a.ZipCode.Equals(address.ZipCode)) is null ? false : true;

        public int GetAddressId(Address address)
        {
            var addressFromDb = FindByCondition(a => a.StreetAddress == address.StreetAddress && a.State == address.State && a.ZipCode == address.ZipCode).SingleOrDefault();

            if(addressFromDb is null)
            {
                return -1;
            }
            return addressFromDb.Id;
        }
    }
}
