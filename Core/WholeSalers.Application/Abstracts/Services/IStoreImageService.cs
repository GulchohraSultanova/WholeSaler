using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Store;
using WholeSalers.Application.DTOs.StoreImage;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IStoreImageService
    {
        Task<List<StoreImageDto>> GetAllAsync();
        Task<StoreImageDto> GetByIdAsync(int id);

        Task CreateAsync(CreateStoreImageDto dto);
        Task DeleteAsync(int id);
    }
}
