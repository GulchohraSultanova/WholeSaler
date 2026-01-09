using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Manufacturer;
using WholeSalers.Application.DTOs.ManufacturerImage;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class ManufactureImageProfile :Profile
    {
        public ManufactureImageProfile()
        {
            CreateMap<ManufacturerImage, ManufacturerImageDto>();
            CreateMap<CreateManufacturerDto, ManufacturerImage>();
        }
    }
}
