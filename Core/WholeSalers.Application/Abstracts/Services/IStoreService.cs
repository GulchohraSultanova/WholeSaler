using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Store;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IStoreService
    {
        Task<List<StoreDto>> GetAllAsync();
        Task<StoreDto> GetByIdAsync(int id);

        Task CreateAsync(CreateStoreDto dto);
        Task UpdateAsync(UpdateStoreDto dto);
        Task DeleteAsync(int id);
    }
}
