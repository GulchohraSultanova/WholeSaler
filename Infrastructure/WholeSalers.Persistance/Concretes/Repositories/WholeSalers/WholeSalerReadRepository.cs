using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.WholeSalers;
using WholeSalers.Domain.Entities;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories.WholeSalers
{
    public class WholeSalerReadRepository : ReadRepository<WholeSaler>, IWholeSalerReadRepository
    {
        public WholeSalerReadRepository(WholeSalersDbContext WholeSalersDbContext) : base(WholeSalersDbContext)
        {
        }
    }
}
