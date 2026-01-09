using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Stores;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Store;
using WholeSalers.Application.DTOs.StoreImage;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreReadRepository _storeReadRepo;
        private readonly IStoreWriteRepository _storeWriteRepo;

        private readonly IWholeSalerService _wholeSalerService;
        private readonly ICategoryService _categoryService;

        private readonly IStoreImageService _storeImageService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public StoreService(
            IStoreReadRepository storeReadRepo,
            IStoreWriteRepository storeWriteRepo,
            IWholeSalerService wholeSalerService,
            ICategoryService categoryService,
            IStoreImageService storeImageService,
            IFileService fileService,
            IMapper mapper)
        {
            _storeReadRepo = storeReadRepo;
            _storeWriteRepo = storeWriteRepo;
            _wholeSalerService = wholeSalerService;
            _categoryService = categoryService;
            _storeImageService = storeImageService;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<StoreDto>> GetAllAsync()
        {
            var stores = await _storeReadRepo.GetAllAsync(
                s => !s.IsDeleted && (s.Category==null || !s.Category.IsDeleted),
                include: q => q
                    .AsSplitQuery()
                    .Include(x => x.WholeSaler)
                    .Include(x => x.Category)
                    .Include(x => x.StoreImages.Where(img => !img.IsDeleted))
            );

            // ✅ Soft-deleted WholeSaler/Category olan store-ları çıxart (null-safe)
            var filtered = stores
                .Where(s => s.WholeSaler != null && !s.WholeSaler.IsDeleted)
                .Where(s => s.Category == null || !s.Category.IsDeleted)
                .ToList();

            return _mapper.Map<List<StoreDto>>(filtered);
        }

        public async Task<StoreDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Store ID!");

            var store = await _storeReadRepo.GetAsync(
                s => s.Id == id && !s.IsDeleted && (s.Category == null || !s.Category.IsDeleted),
                include: q => q
                    .AsSplitQuery()
                    .Include(x => x.WholeSaler)
                    .Include(x => x.Category)
                    .Include(x => x.StoreImages.Where(img => !img.IsDeleted))
            );

            if (store == null)
                throw new GlobalAppException("Store tapılmadı!");

            if (store.WholeSaler == null || store.WholeSaler.IsDeleted)
                throw new GlobalAppException("Bu store-a aid WholeSaler silinib!");

            if (store.Category == null || store.Category.IsDeleted)
                throw new GlobalAppException("Bu store-a aid Category silinib!");

            return _mapper.Map<StoreDto>(store);
        }

        public async Task CreateAsync(CreateStoreDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            if (dto.WholeSalerId <= 0)
                throw new GlobalAppException("Yanlış WholeSaler ID!");


            // ⛔ CATEGORY ARTIQ MƏCBURİ DEYİL
            if (dto.CategoryId.HasValue)
            {
                if (dto.CategoryId.Value <= 0)
                    throw new GlobalAppException("Yanlış Category ID!");

                await _categoryService.GetByIdAsync(dto.CategoryId.Value);
            }


            // wholesaler must exist
            await _wholeSalerService.GetByIdAsync(dto.WholeSalerId);


            if (dto.CardImage == null || dto.CardImage.Length == 0)
                throw new GlobalAppException("CardImage boş ola bilməz!");


            var store = _mapper.Map<Store>(dto);

            // mapper CategoryId null-u normal map edəcək
            store.CardImage = await _fileService.UploadFile(dto.CardImage, "stores");


            await _storeWriteRepo.AddAsync(store);
            await _storeWriteRepo.CommitAsync();


            // gallery images
            if (dto.StoreImages != null && dto.StoreImages.Any())
            {
                foreach (var file in dto.StoreImages)
                {
                    if (file == null || file.Length == 0) continue;

                    var fileName = await _fileService.UploadFile(file, "store_images");

                    await _storeImageService.CreateAsync(new CreateStoreImageDto
                    {
                        StoreId = store.Id,
                        Name = fileName
                    });
                }
            }
        }


        public async Task UpdateAsync(UpdateStoreDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new GlobalAppException("Yanlış Store ID!");

            // ✅ 1) Tracked store (update üçün)
            var store = await _storeReadRepo.GetAsync(
                s => s.Id == dto.Id && !s.IsDeleted,
                enableTracking: true
            ) ?? throw new GlobalAppException("Store tapılmadı!");

            // ✅ 2) StoreImages siyahısını NO-TRACKING şəkildə ayrıca götürürük (tracking conflict olmasın)
            var storeForImages = await _storeReadRepo.GetAsync(
                s => s.Id == dto.Id && !s.IsDeleted,
                include: q => q
                    .AsSplitQuery()
                    .Include(x => x.StoreImages), // burada filter etməsək də olar, sonra özümüz filter edirik
                enableTracking: false
            ) ?? throw new GlobalAppException("Store tapılmadı!");

            // ✅ validate wholesaler if changed
            if (dto.WholeSalerId.HasValue)
            {
                if (dto.WholeSalerId.Value <= 0)
                    throw new GlobalAppException("Yanlış WholeSaler ID!");

                await _wholeSalerService.GetByIdAsync(dto.WholeSalerId.Value);
            }

            // ✅ validate category if changed
            if (dto.CategoryId.HasValue)
            {
                if (dto.CategoryId.Value <= 0)
                    throw new GlobalAppException("Yanlış Category ID!");

                await _categoryService.GetByIdAsync(dto.CategoryId.Value);
            }

            // keep old values for partial update
            var oldTitle = store.Title;
            var oldSubTitle = store.SubTitle;
            var oldMobilePhone = store.MobilePhone;
            var oldWhatsappPhone = store.WhatsappPhone;
            var oldTikTokLink = store.TikTokLink;
            var oldWebSiteUrl = store.WebSiteUrl;
            var oldInstaLink = store.InstaLink;
            var oldInLocation = store.InLocation;
            var oldMapLocation = store.MapLocation;
            var oldYoutubeLink = store.YoutubeLink;
            var oldWholeSalerId = store.WholeSalerId;
            var oldCategoryId = store.CategoryId;

            _mapper.Map(dto, store);

            if (dto.Title == null) store.Title = oldTitle;
            if (dto.SubTitle == null) store.SubTitle = oldSubTitle;
            if (dto.MobilePhone == null) store.MobilePhone = oldMobilePhone;
            if (dto.WhatsappPhone == null) store.WhatsappPhone = oldWhatsappPhone;
            if (dto.TikTokLink == null) store.TikTokLink = oldTikTokLink;
            if (dto.WebSiteUrl == null) store.WebSiteUrl = oldWebSiteUrl;
            if (dto.InstaLink == null) store.InstaLink = oldInstaLink;
            if (dto.InLocation == null) store.InLocation = oldInLocation;
            if (dto.MapLocation == null) store.MapLocation = oldMapLocation;
            if (dto.YoutubeLink == null) store.YoutubeLink = oldYoutubeLink;

            if (!dto.WholeSalerId.HasValue) store.WholeSalerId = oldWholeSalerId;
            if (!dto.CategoryId.HasValue) store.CategoryId = oldCategoryId;

            // ✅ update card image
            if (dto.CardImage != null && dto.CardImage.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(store.CardImage))
                    await _fileService.DeleteFile("stores", store.CardImage);

                store.CardImage = await _fileService.UploadFile(dto.CardImage, "stores");
            }

            // ✅ delete selected gallery images (allowedIds NO-TRACKING storeForImages üstündən)
            if (dto.DeletedStoreImageIds != null && dto.DeletedStoreImageIds.Any())
            {
                var allowedIds = storeForImages.StoreImages?
                    .Where(x => !x.IsDeleted && dto.DeletedStoreImageIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList() ?? new List<int>();

                foreach (var imageId in allowedIds)
                    await _storeImageService.DeleteAsync(imageId);
            }

            // ✅ add new gallery images
            if (dto.StoreImages != null && dto.StoreImages.Any())
            {
                foreach (var file in dto.StoreImages)
                {
                    if (file == null || file.Length == 0) continue;

                    var fileName = await _fileService.UploadFile(file, "store_images");

                    await _storeImageService.CreateAsync(new CreateStoreImageDto
                    {
                        StoreId = store.Id,
                        Name = fileName
                    });
                }
            }

            await _storeWriteRepo.UpdateAsync(store);
            await _storeWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Store ID!");

            var store = await _storeReadRepo.GetAsync(
                s => s.Id == id && !s.IsDeleted
             
            ) ?? throw new GlobalAppException("Store tapılmadı!");

            if (!string.IsNullOrWhiteSpace(store.CardImage))
                await _fileService.DeleteFile("stores", store.CardImage);

            if (store.StoreImages != null && store.StoreImages.Any())
            {
                foreach (var img in store.StoreImages.Where(x => !x.IsDeleted))
                    await _storeImageService.DeleteAsync(img.Id);
            }

            await _storeWriteRepo.SoftDeleteAsync(store);
            await _storeWriteRepo.CommitAsync();
        }
    }
}
