using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSalers.Application.DTOs.Manufacturer
{
    public class UpdateManufacturerDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? CardImage { get; set; }
        public string? MobilePhone { get; set; }

        public string? WhatsappPhone { get; set; }
        public string? TikTokLink { get; set; }
        public string? WebSiteUrl { get; set; }
        public string? InstaLink { get; set; }
        public string? InLocation { get; set; }
        public string? MapLocation { get; set; }
        public string? YoutubeLink { get; set; }
        public List<IFormFile>? ManufacturerImages { get; set; }
        public List<int>? DeletedManufacturerImageIds { get; set; }
    }
}
