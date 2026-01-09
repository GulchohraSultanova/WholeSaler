using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Category;
using WholeSalers.Application.DTOs.StoreImage;
using WholeSalers.Application.DTOs.WholeSaler;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.DTOs.Store
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string CardImage { get; set; }
        public string? MobilePhone { get; set; }

        public string? WhatsappPhone { get; set; }
        public string? TikTokLink { get; set; }
        public string? WebSiteUrl { get; set; }
        public string? InstaLink { get; set; }
        public string InLocation { get; set; }
        public string? MapLocation { get; set; }
        public string? YoutubeLink { get; set; }
        public int WholeSalerId { get; set; }
        public string? WholeSalerTitle { get; set; }
        public CategoryShortDto CategoryDetail { get; set; }
        public List<StoreImageDto>? StoreImages { get; set; }      
    }
}
