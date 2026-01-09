using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSalers.Application.DTOs.ManufacturerImage
{
    public class ManufacturerImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
     
    }
}
