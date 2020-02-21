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

    }
}
