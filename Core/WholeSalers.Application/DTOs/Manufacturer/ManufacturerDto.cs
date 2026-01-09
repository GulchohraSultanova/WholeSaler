using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.ManufacturerImage;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.DTOs.Manufacturer
{
    public class ManufacturerDto
    {
        public int Id { get; set; }
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
        public List<ManufacturerImageDto>? ManufacturerImages { get; set; }
    }
}
