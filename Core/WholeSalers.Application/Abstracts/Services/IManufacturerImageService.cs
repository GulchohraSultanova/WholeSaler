using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.ManufacturerImage;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IManufacturerImageService
    {
        Task<List<ManufacturerImageDto>> GetAllAsync();
        Task<ManufacturerImageDto> GetByIdAsync(int id);

        Task CreateAsync(int manufacturerId,IFormFile formFile);
        Task DeleteAsync(int id);
    }
}
