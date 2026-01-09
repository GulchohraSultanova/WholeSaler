using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.WholeSaler;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IWholeSalerService
    {
        Task<List<WholeSalerDto>> GetAllAsync();
        Task<WholeSalerDto> GetByIdAsync(int id);

        Task CreateAsync(CreateWholeSalerDto dto);
        Task UpdateAsync(UpdateWholeSalerDto dto);
        Task DeleteAsync(int id);
    }
}
