
using WholeSalers.Domain.Entities.Common;

namespace WholeSalers.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<Store>? Stores { get; set; }
    }
}
