using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Slider;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface ISliderService
    {
        Task<List<SliderDto>> GetAllAsync();
        Task<SliderDto> GetByIdAsync(int? id);
        Task CreateAsync(CreateSliderDto dto);
        Task UpdateAsync(UpdateSliderDto dto);
        Task DeleteAsync(int id);
    }
}
