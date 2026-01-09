using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Slider;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class SliderProfile:Profile
    {
        public SliderProfile()
        {
            CreateMap<Slider,SliderDto>().ReverseMap();
            CreateMap<Slider, CreateSliderDto>().ReverseMap();
            CreateMap<Slider, UpdateSliderDto>().ReverseMap();

        }
    }
}
