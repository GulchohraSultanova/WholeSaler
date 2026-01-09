using AutoMapper;
using WholeSalers.Application.DTOs.Store;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Store, StoreDto>()
                .ForMember(d => d.WholeSalerTitle,
                    opt => opt.MapFrom(s => s.WholeSaler != null ? s.WholeSaler.Title : null))
                     .ForMember(d => d.CategoryDetail,
                    opt => opt.MapFrom(s => s.Category)); 

            CreateMap<CreateStoreDto, Store>()
                .ForMember(d => d.CardImage, opt => opt.Ignore())
                .ForMember(d => d.StoreImages, opt => opt.Ignore());

            CreateMap<UpdateStoreDto, Store>()
                .ForMember(d => d.CardImage, opt => opt.Ignore())
                .ForMember(d => d.StoreImages, opt => opt.Ignore())
                .ForMember(d => d.WholeSalerId, opt =>
                {
                    opt.PreCondition(s => s.WholeSalerId.HasValue);
                    opt.MapFrom(s => s.WholeSalerId.Value);
                })
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
