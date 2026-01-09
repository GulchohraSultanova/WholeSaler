using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.DTOs.Store
{
    public class CreateStoreDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IFormFile CardImage { get; set; }
        public string? MobilePhone { get; set; }
        public string? WhatsappPhone { get; set; }
        public string? TikTokLink { get; set; }
        public string? WebSiteUrl { get; set; }
        public string? InstaLink { get; set; }
        public string InLocation { get; set; }
        public string? MapLocation { get; set; }
        public string? YoutubeLink { get; set; }
        public int WholeSalerId { get; set; }
        public int? CategoryId { get; set; }

        public List<IFormFile>? StoreImages { get; set; }
    }
}
