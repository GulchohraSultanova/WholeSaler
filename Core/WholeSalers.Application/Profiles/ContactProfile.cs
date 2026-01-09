using AutoMapper;
using WholeSalers.Application.DTOs.Contact;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Profiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            // Entity -> Dto (GetAll, GetById)
            CreateMap<Contact, ContactDto>();

            // CreateDto -> Entity
            CreateMap<CreateContactDto, Contact>();
        }
    }
}
