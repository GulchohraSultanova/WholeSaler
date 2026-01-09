using AutoMapper;
using WholeSalers.Application.DTOs.Category;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Entity -> Dto
            CreateMap<Category, CategoryDto>();

            // Entity -> ShortDto (StoreDto içində istifadə olunur)
            CreateMap<Category, CategoryShortDto>();

            // CreateDto -> Entity
            // Icon IFormFile olduğu üçün burada map etmirik (upload service-də olacaq)
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(d => d.Icon, opt => opt.Ignore())
                .ForMember(d => d.Stores, opt => opt.Ignore());

            // UpdateDto -> Entity (partial update)
            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(d => d.Icon, opt => opt.Ignore())
                .ForMember(d => d.Stores, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
