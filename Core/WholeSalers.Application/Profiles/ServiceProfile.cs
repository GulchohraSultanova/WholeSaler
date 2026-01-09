using AutoMapper;
using WholeSalers.Application.DTOs.Service;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            // Entity -> Dto
            CreateMap<Service, ServiceDto>();

            // CreateDto -> Entity
            CreateMap<CreateServiceDto, Service>()
                .ForMember(d => d.CardImage,
                    opt => opt.MapFrom(s => s.CardImage != null ? s.CardImage.FileName : null));

            // UpdateDto -> Entity
            CreateMap<UpdateServiceDto, Service>()
                // CardImage null-dursa, entity-dəki CardImage dəyişməsin
                .ForMember(d => d.CardImage, opt =>
                {
                    opt.PreCondition(s => s.CardImage != null);
                    opt.MapFrom(s => s.CardImage.FileName);
                });
        }
    }
}
