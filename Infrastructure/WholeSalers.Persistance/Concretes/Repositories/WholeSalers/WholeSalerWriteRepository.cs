using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Contacts;
using WholeSalers.Application.Abstracts.Repositories.WholeSalers;
using WholeSalers.Domain.Entities;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories.WholeSalers
{
    public class WholeSalerWriteRepository : WriteRepository<WholeSaler>, IWholeSalerWriteRepository
    {
        public WholeSalerWriteRepository(WholeSalersDbContext WholeSalersDbContext) : base(WholeSalersDbContext)
        {
        }
    }
}
