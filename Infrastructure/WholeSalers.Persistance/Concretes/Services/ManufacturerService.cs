using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Manufacturers;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Manufacturer;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerReadRepository _manufacturerReadRepo;
        private readonly IManufacturerWriteRepository _manufacturerWriteRepo;
        private readonly IManufacturerImageService _manufacturerImageService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ManufacturerService(
            IManufacturerReadRepository manufacturerReadRepo,
            IManufacturerWriteRepository manufacturerWriteRepo,
            IManufacturerImageService manufacturerImageService,
            IFileService fileService,
            IMapper mapper)
        {
            _manufacturerReadRepo = manufacturerReadRepo;
            _manufacturerWriteRepo = manufacturerWriteRepo;
            _manufacturerImageService = manufacturerImageService;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<ManufacturerDto>> GetAllAsync()
        {
            var manufacturers = await _manufacturerReadRepo.GetAllAsync(
                m => !m.IsDeleted,
                include: q => q
                    .AsSplitQuery()
                    .Include(x => x.ManufacturerImages.Where(img => !img.IsDeleted))
            );

            return _mapper.Map<List<ManufacturerDto>>(manufacturers.ToList());
        }

        public async Task<ManufacturerDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Manufacturer ID!");

            var manufacturer = await _manufacturerReadRepo.GetAsync(
                m => m.Id == id && !m.IsDeleted,
                include: q => q
                    .AsSplitQuery()
                    .Include(x => x.ManufacturerImages.Where(img => !img.IsDeleted))
            );

            if (manufacturer == null)
                throw new GlobalAppException("Manufacturer tapılmadı!");

            return _mapper.Map<ManufacturerDto>(manufacturer);
        }

        public async Task CreateAsync(CreateManufacturerDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            var manufacturer = _mapper.Map<Manufacturer>(dto);

            // ✅ upload card image
            if (dto.CardImage != null && dto.CardImage.Length > 0)
                manufacturer.CardImage = await _fileService.UploadFile(dto.CardImage, "manufacturers");
            else
                throw new GlobalAppException("CardImage boş ola bilməz!");

            // ✅ save manufacturer to get Id
            await _manufacturerWriteRepo.AddAsync(manufacturer);
            await _manufacturerWriteRepo.CommitAsync();

            // ✅ append gallery images
            if (dto.ManufacturerImages != null && dto.ManufacturerImages.Any())
            {
                foreach (var file in dto.ManufacturerImages)
                {
                    if (file == null || file.Length == 0) continue;
                    await _manufacturerImageService.CreateAsync(manufacturer.Id, file);
                }
            }
        }

        public async Task UpdateAsync(UpdateManufacturerDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new GlobalAppException("Yanlış Manufacturer ID!");

            // ✅ IMPORTANT: include images so we can validate allowed ids
            var manufacturer = await _manufacturerReadRepo.GetAsync(
                m => m.Id == dto.Id && !m.IsDeleted,
                              enableTracking: true

            ) ?? throw new GlobalAppException("Manufacturer tapılmadı!");
            var manufacturerForImages = await _manufacturerReadRepo.GetAsync(
          s => s.Id == dto.Id && !s.IsDeleted,
          include: q => q
              .AsSplitQuery()
              .Include(x => x.ManufacturerImages), // burada filter etməsək də olar, sonra özümüz filter edirik
          enableTracking: false
      ) ?? throw new GlobalAppException("Store tapılmadı!");

            // keep old values for partial update
            var oldName = manufacturer.Name;
            var oldDescription = manufacturer.Description;
            var oldMobilePhone = manufacturer.MobilePhone;
            var oldWhatsappPhone = manufacturer.WhatsappPhone;
            var oldTikTokLink = manufacturer.TikTokLink;
            var oldWebSiteUrl = manufacturer.WebSiteUrl;
            var oldInstaLink = manufacturer.InstaLink;
            var oldInLocation = manufacturer.InLocation;
            var oldMapLocation = manufacturer.MapLocation;
            var oldYoutubeLink = manufacturer.YoutubeLink;

            _mapper.Map(dto, manufacturer);

            if (dto.Name == null) manufacturer.Name = oldName;
            if (dto.Description == null) manufacturer.Description = oldDescription;
            if (dto.MobilePhone == null) manufacturer.MobilePhone = oldMobilePhone;
            if (dto.WhatsappPhone == null) manufacturer.WhatsappPhone = oldWhatsappPhone;
            if (dto.TikTokLink == null) manufacturer.TikTokLink = oldTikTokLink;
            if (dto.WebSiteUrl == null) manufacturer.WebSiteUrl = oldWebSiteUrl;
            if (dto.InstaLink == null) manufacturer.InstaLink = oldInstaLink;
            if (dto.InLocation == null) manufacturer.InLocation = oldInLocation;
            if (dto.MapLocation == null) manufacturer.MapLocation = oldMapLocation;
            if (dto.YoutubeLink == null) manufacturer.YoutubeLink = oldYoutubeLink;

            // ✅ update card image
            if (dto.CardImage != null && dto.CardImage.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(manufacturer.CardImage))
                    await _fileService.DeleteFile("manufacturers", manufacturer.CardImage);

                manufacturer.CardImage = await _fileService.UploadFile(dto.CardImage, "manufacturers");
            }

            // ✅ delete selected gallery images
            if (dto.DeletedManufacturerImageIds != null)
            {
                
                // only images that belong to THIS manufacturer and not deleted
                var allowedIds = manufacturerForImages.ManufacturerImages?
                    .Where(i => !i.IsDeleted && dto.DeletedManufacturerImageIds.Contains(i.Id))
                    .Select(i => i.Id)
                    .ToList() ?? new List<int>();

                foreach (var imgId in allowedIds)
                    await _manufacturerImageService.DeleteAsync(imgId);
            }

            // ✅ add new gallery images (append)
            if (dto.ManufacturerImages != null && dto.ManufacturerImages.Any())
            {
                foreach (var file in dto.ManufacturerImages)
                {
                    if (file == null || file.Length == 0) continue;
                    await _manufacturerImageService.CreateAsync(manufacturer.Id, file);
                }
            }

            await _manufacturerWriteRepo.UpdateAsync(manufacturer);
            await _manufacturerWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Manufacturer ID!");

            var manufacturer = await _manufacturerReadRepo.GetAsync(
                m => m.Id == id && !m.IsDeleted) ?? throw new GlobalAppException("Manufacturer tapılmadı!");

            // delete card image file
            if (!string.IsNullOrWhiteSpace(manufacturer.CardImage))
                await _fileService.DeleteFile("manufacturers", manufacturer.CardImage);

            // delete all gallery images
            if (manufacturer.ManufacturerImages != null && manufacturer.ManufacturerImages.Any())
            {
                foreach (var img in manufacturer.ManufacturerImages.Where(x => !x.IsDeleted))
                    await _manufacturerImageService.DeleteAsync(img.Id);
            }

            await _manufacturerWriteRepo.SoftDeleteAsync(manufacturer);
            await _manufacturerWriteRepo.CommitAsync();
        }
    }
}
