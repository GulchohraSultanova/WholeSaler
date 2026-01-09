using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Manufacturers;
using WholeSalers.Domain.Entities;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories.Manufacturers
{
    public class ManufacturerReadRepository : ReadRepository<Manufacturer>, IManufacturerReadRepository
    {
        public ManufacturerReadRepository(WholeSalersDbContext WholeSalersDbContext) : base(WholeSalersDbContext)
        {
        }
    }
}
