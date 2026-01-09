using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Manufacturer;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IManufacturerService
    {
        Task<List<ManufacturerDto>> GetAllAsync();
        Task<ManufacturerDto> GetByIdAsync(int id);

        Task CreateAsync(CreateManufacturerDto dto);
        Task UpdateAsync(UpdateManufacturerDto dto);
        Task DeleteAsync(int id);
    }
}
