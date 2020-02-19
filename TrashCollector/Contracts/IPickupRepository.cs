using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Models;

namespace TrashCollector.Contracts
{
    public interface IPickupRepository : IRepositoryBase<Pickup>
    {
        Pickup GetPickup(int id);
        //int GetPickupId(Pickup pickup);
        void CreatePickup(Pickup pickup);
        void UpdatePickup(Pickup pickup);



    }
}
