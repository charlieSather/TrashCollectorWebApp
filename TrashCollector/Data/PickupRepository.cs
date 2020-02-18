using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollector.Contracts;
using TrashCollector.Models;

namespace TrashCollector.Data
{
    public class PickupRepository : RepositoryBase<Pickup>, IPickupRepository
    {
        public PickupRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext) 
        {
        }
    }
}
