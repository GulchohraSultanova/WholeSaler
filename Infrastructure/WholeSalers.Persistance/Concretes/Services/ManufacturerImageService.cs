using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.ManufacturerImages;
using WholeSalers.Application.Abstracts.Repositories.Manufacturers;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.ManufacturerImage;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class ManufacturerImageService : IManufacturerImageService
    {
        private readonly IManufacturerImageReadRepository _manufacturerImageReadRepo;
        private readonly IManufacturerImageWriteRepository _manufacturerImageWriteRepo;
        private readonly IFileService _fileService;
        private readonly IManufacturerReadRepository _manufacturerReadRepo;
        private readonly IMapper _mapper;

        public ManufacturerImageService(
            IManufacturerImageReadRepository manufacturerImageReadRepo,
            IManufacturerImageWriteRepository manufacturerImageWriteRepo,
            IFileService fileService,
            IManufacturerReadRepository manufacturerReadRepo,
            IMapper mapper)
        {
            _manufacturerImageReadRepo = manufacturerImageReadRepo;
            _manufacturerImageWriteRepo = manufacturerImageWriteRepo;
            _fileService = fileService;
            _manufacturerReadRepo = manufacturerReadRepo;
            _mapper = mapper;
        }

        public async Task CreateAsync(int manufacturerId, IFormFile formFile)
        {
            if (manufacturerId <= 0)
                throw new GlobalAppException("Yanlış Manufacturer ID!");

            if (formFile == null || formFile.Length == 0)
                throw new GlobalAppException("Şəkil faylı boş ola bilməz!");

            // ✅ validate manufacturer exists & not deleted
            var manufacturer = await _manufacturerReadRepo.GetAsync(m => m.Id == manufacturerId && !m.IsDeleted);
            if (manufacturer == null)
                throw new GlobalAppException("Manufacturer tapılmadı və ya silinib!");

            // ✅ upload file (folder+file burada yaranır)
            var storedFileName = await _fileService.UploadFile(formFile, "manufacturer_images");

            // ✅ save db record with stored name
            var entity = new ManufacturerImage
            {
                ManufacturerId = manufacturerId,
                Name = storedFileName
            };

            await _manufacturerImageWriteRepo.AddAsync(entity);
            await _manufacturerImageWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış ManufacturerImage ID!");

            var img = await _manufacturerImageReadRepo.GetAsync(i => i.Id == id && !i.IsDeleted)
                      ?? throw new GlobalAppException("ManufacturerImage tapılmadı!");

            // ✅ delete file from disk
            if (!string.IsNullOrWhiteSpace(img.Name))
                await _fileService.DeleteFile("manufacturer_images", img.Name);

            // ✅ soft delete record
            await _manufacturerImageWriteRepo.SoftDeleteAsync(img);
            await _manufacturerImageWriteRepo.CommitAsync();
        }

        public async Task<List<ManufacturerImageDto>> GetAllAsync()
        {
            var images = await _manufacturerImageReadRepo.GetAllAsync(i => !i.IsDeleted);
            return _mapper.Map<List<ManufacturerImageDto>>(images.ToList());
        }

        public async Task<ManufacturerImageDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış ManufacturerImage ID!");

            var img = await _manufacturerImageReadRepo.GetAsync(i => i.Id == id && !i.IsDeleted)
                      ?? throw new GlobalAppException("ManufacturerImage tapılmadı!");

            return _mapper.Map<ManufacturerImageDto>(img);
        }
    }
}
