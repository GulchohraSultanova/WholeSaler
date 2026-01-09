using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSalers.Application.DTOs.Slider
{
    public class UpdateSliderDto
    {
        public int Id { get; set; }
        public IFormFile? ImageName { get; set; }
    }
}
