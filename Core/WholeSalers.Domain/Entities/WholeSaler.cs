using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities.Common;

namespace WholeSalers.Domain.Entities
{
    public class WholeSaler:BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string CardImage { get; set; }
        public string? Location { get; set; }
        public List<Store>? Stores { get; set; }

    }
}
