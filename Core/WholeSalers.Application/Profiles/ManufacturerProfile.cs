using AutoMapper;
using WholeSalers.Application.DTOs.Manufacturer;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            // ✅ Entity -> Dto (ManufacturerImages də avtomatik map olunur, əgər ManufacturerImageProfile var)
            CreateMap<Manufacturer, ManufacturerDto>();

            // ✅ CreateDto -> Entity
            // CardImage IFormFile olduğu üçün map etmirik (service upload edəcək)
            // ManufacturerImages də map edilmir (ayrı cədvəl / service ilə əlavə olunur)
            CreateMap<CreateManufacturerDto, Manufacturer>()
                .ForMember(d => d.CardImage, opt => opt.Ignore())
                .ForMember(d => d.ManufacturerImages, opt => opt.Ignore());

            // ✅ UpdateDto -> Entity (partial update)
            CreateMap<UpdateManufacturerDto, Manufacturer>()
                .ForMember(d => d.CardImage, opt => opt.Ignore())
                .ForMember(d => d.ManufacturerImages, opt => opt.Ignore())
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
