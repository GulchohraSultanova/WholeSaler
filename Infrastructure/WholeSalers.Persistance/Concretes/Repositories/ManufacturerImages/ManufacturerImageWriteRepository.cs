using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.ManufacturerImages;
using WholeSalers.Domain.Entities;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories.ManufacturerImages
{
    public class ManufacturerImageWriteRepository : WriteRepository<ManufacturerImage>, IManufacturerImageWriteRepository
    {
        public ManufacturerImageWriteRepository(WholeSalersDbContext WholeSalersDbContext) : base(WholeSalersDbContext)
        {
        }
    }
}
