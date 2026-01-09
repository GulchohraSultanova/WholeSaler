using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities.Common;

namespace WholeSalers.Domain.Entities
{
    public class Manufacturer:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CardImage { get; set; }
        public string? MobilePhone { get; set; }

        public string? WhatsappPhone { get; set; }
        public string? TikTokLink { get; set; }
        public string? WebSiteUrl { get; set; }
        public string? InstaLink { get; set; }
        public string InLocation { get; set; }
        public string? MapLocation { get; set; }
        public string? YoutubeLink { get; set; }
        public List<ManufacturerImage>? ManufacturerImages { get; set; }

    }
}
