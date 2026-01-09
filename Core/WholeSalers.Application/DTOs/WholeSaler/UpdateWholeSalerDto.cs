using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.DTOs.WholeSaler
{
    public class UpdateWholeSalerDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public IFormFile? CardImage { get; set; }
        public string? Location { get; set; }
    }
}
