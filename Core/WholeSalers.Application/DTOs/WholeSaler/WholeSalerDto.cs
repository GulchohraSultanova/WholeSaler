using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Store;
using WholeSalers.Application.DTOs.StoreImage;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.DTOs.WholeSaler
{
    public class WholeSalerDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string CardImage { get; set; }
        public string? Location { get; set; }
        public List<StoreDto>? Stores { get; set; }
    }
}
