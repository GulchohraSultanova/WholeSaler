using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Contacts;
using WholeSalers.Domain.Entities;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories.Contacts
{
    public class ContactWriteRepository : WriteRepository<Contact>, IContactWriteRepository
    {
        public ContactWriteRepository(WholeSalersDbContext WholeSalersDbContext) : base(WholeSalersDbContext)
        {
        }
    }
}
