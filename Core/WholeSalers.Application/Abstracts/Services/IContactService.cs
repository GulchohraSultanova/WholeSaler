using System.Collections.Generic;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Contact;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllAsync();
        Task<ContactDto> GetByIdAsync(int id);
        Task CreateAsync(CreateContactDto dto);

        Task DeleteAsync(int id);
    }
}
