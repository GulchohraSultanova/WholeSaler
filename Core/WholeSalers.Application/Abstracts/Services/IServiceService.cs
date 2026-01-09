using System.Collections.Generic;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Service;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IServiceService
    {
        Task<List<ServiceDto>> GetAllAsync();
        Task<ServiceDto> GetByIdAsync(int id);

        Task CreateAsync(CreateServiceDto dto);
        Task UpdateAsync(UpdateServiceDto dto);
        Task DeleteAsync(int id);
    }
}
