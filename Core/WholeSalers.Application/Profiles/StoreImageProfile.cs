using AutoMapper;
using WholeSalers.Application.DTOs.StoreImage;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class StoreImageProfile : Profile
    {
        public StoreImageProfile()
        {
            CreateMap<StoreImage, StoreImageDto>();
            CreateMap<CreateStoreImageDto, StoreImage>();
        }
    }
}
