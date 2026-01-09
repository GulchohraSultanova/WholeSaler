using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities.Common;

namespace WholeSalers.Domain.Entities
{
    public class StoreImage:BaseEntity
    {
        public string Name { get; set; }
        public int StoreId { get; set; }
        public Store? Store { get; set; }
    }
}
