using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Services;
using WholeSalers.Application.Abstracts.Repositories.Stores;
using WholeSalers.Domain.Entities;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories.Stores
{
    public class StoreReadRepository : ReadRepository<Store>, IStoreReadRepository
    {
        public StoreReadRepository(WholeSalersDbContext WholeSalersDbContext) : base(WholeSalersDbContext)
        {
        }
    }
}
