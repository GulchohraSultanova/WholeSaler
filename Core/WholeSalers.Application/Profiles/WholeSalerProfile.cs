using AutoMapper;
using WholeSalers.Application.DTOs.WholeSaler;
using WholeSalers.Domain.Entities;
using System.Linq;

namespace WholeSalers.Application.Profiles
{
    public class WholeSalerProfile : Profile
    {
        public WholeSalerProfile()
        {
            // WholeSaler -> WholeSalerDto (include Stores)
            CreateMap<WholeSaler, WholeSalerDto>()
                .ForMember(d => d.Stores, opt =>
                    opt.MapFrom(s => s.Stores != null
                        ? s.Stores.Where(x => !x.IsDeleted).ToList()
                        : null));

            // CreateDto -> Entity (upload in service)
            CreateMap<CreateWholeSalerDto, WholeSaler>()
                .ForMember(d => d.CardImage, opt => opt.Ignore())
                .ForMember(d => d.Stores, opt => opt.Ignore());

            // UpdateDto -> Entity (partial update)
            CreateMap<UpdateWholeSalerDto, WholeSaler>()
                .ForMember(d => d.CardImage, opt => opt.Ignore())
                .ForMember(d => d.Stores, opt => opt.Ignore())
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
