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

        public void CreatePickup(Pickup pickup) => Create(pickup);
        public Pickup GetPickup(int id) => FindByCondition(p => p.Id == id).FirstOrDefault();
        public IQueryable<Pickup> GetPickups() => FindAll();
        public void UpdatePickup(Pickup pickup) => Update(pickup);
    }
}
