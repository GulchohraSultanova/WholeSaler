using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Abstracts.Repositories.Contacts
{
    public interface IContactWriteRepository:IWriteRepository<Contact>
    {
    }
}
