using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Sliders;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Category;
using WholeSalers.Application.DTOs.Slider;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderReadRepository _sliderReadRepo;
        private readonly ISliderWriteRepository _sliderWriteRepo;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public SliderService(ISliderReadRepository sliderReadRepo, ISliderWriteRepository sliderWriteRepo, IFileService fileService, IMapper mapper)
        {
            _sliderReadRepo = sliderReadRepo;
            _sliderWriteRepo = sliderWriteRepo;
            _fileService = fileService;
            _mapper = mapper;
        }


        public async Task CreateAsync(CreateSliderDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

     

            if (dto.ImageName == null || dto.ImageName.Length == 0)
                throw new GlobalAppException("Image boş ola bilməz!");

           Slider slider = new()
           {
               ImageName = dto.ImageName.Name,
           };

            // upload icon file -> save file name/path to entity.Icon
             slider.ImageName = await _fileService.UploadFile(dto.ImageName, "sliders");
         

            await _sliderWriteRepo.AddAsync(slider);
            await _sliderWriteRepo.CommitAsync();
        }

        public async  Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Slider ID!");

            var slider = await _sliderReadRepo.GetAsync(c => c.Id == id && !c.IsDeleted)
                           ?? throw new GlobalAppException("Slider tapılmadı!");

            // optional: delete icon file
            if (!string.IsNullOrWhiteSpace(slider.ImageName))
                await _fileService.DeleteFile("sliders", slider.ImageName);

            await _sliderWriteRepo.SoftDeleteAsync(slider);
            await _sliderWriteRepo.CommitAsync();
        }

        public async  Task<List<SliderDto>> GetAllAsync()
        {
            var sliders = await _sliderReadRepo.GetAllAsync(
            c => !c.IsDeleted );

            return _mapper.Map<List<SliderDto>>(sliders.ToList());
        }

        public async Task<SliderDto> GetByIdAsync(int? id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Slider ID!");

            var slider = await _sliderReadRepo.GetAsync(
                c => c.Id == id && !c.IsDeleted
             
            );

            if (slider == null)
                throw new GlobalAppException("Slider tapılmadı!");

            return _mapper.Map<SliderDto>(slider);
        }

        public async Task UpdateAsync(UpdateSliderDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new GlobalAppException("Yanlış Slider ID!");

            var slider = await _sliderReadRepo.GetAsync(c => c.Id == dto.Id && !c.IsDeleted)
                           ?? throw new GlobalAppException("Slider tapılmadı!");

         
            var oldImage = slider.ImageName;

      

            // icon update
            if (dto.ImageName != null && dto.ImageName.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(oldImage))
                    await _fileService.DeleteFile("sliders", oldImage);

                slider.ImageName = await _fileService.UploadFile(dto.ImageName, "sliders");
            }
            else
            {
                slider.ImageName = oldImage;
            }

            await _sliderWriteRepo.UpdateAsync(slider);
            await _sliderWriteRepo.CommitAsync();
        }
    }
}
