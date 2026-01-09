using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.StoreImages;
using WholeSalers.Application.Abstracts.Repositories.Stores;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.StoreImage;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class StoreImageService : IStoreImageService
    {
        private readonly IStoreImageReadRepository _storeImageReadRepo;
        private readonly IStoreImageWriteRepository _storeImageWriteRepo;
        private readonly IStoreReadRepository _storeReadRepo;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public StoreImageService(
            IStoreImageReadRepository storeImageReadRepo,
            IStoreImageWriteRepository storeImageWriteRepo,
            IStoreReadRepository storeReadRepo,
            IFileService fileService,
            IMapper mapper)
        {
            _storeImageReadRepo = storeImageReadRepo;
            _storeImageWriteRepo = storeImageWriteRepo;
            _storeReadRepo = storeReadRepo;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<StoreImageDto>> GetAllAsync()
        {
            var images = await _storeImageReadRepo.GetAllAsync(i => !i.IsDeleted);
            return _mapper.Map<List<StoreImageDto>>(images.ToList());
        }

        public async Task<StoreImageDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış StoreImage ID!");

            var img = await _storeImageReadRepo.GetAsync(i => i.Id == id && !i.IsDeleted)
                      ?? throw new GlobalAppException("StoreImage tapılmadı!");

            return _mapper.Map<StoreImageDto>(img);
        }

        public async Task CreateAsync(CreateStoreImageDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            if (dto.StoreId <= 0)
                throw new GlobalAppException("Yanlış Store ID!");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new GlobalAppException("Şəkil adı (Name) boş ola bilməz!");

            // ✅ validate store exists & not deleted
            var store = await _storeReadRepo.GetAsync(s => s.Id == dto.StoreId && !s.IsDeleted);
            if (store == null)
                throw new GlobalAppException("Store tapılmadı və ya silinib!");

            var entity = new StoreImage
            {
                StoreId = dto.StoreId,
                Name = dto.Name.Trim()
            };

            await _storeImageWriteRepo.AddAsync(entity);
            await _storeImageWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış StoreImage ID!");

            var img = await _storeImageReadRepo.GetAsync(i => i.Id == id && !i.IsDeleted)
                      ?? throw new GlobalAppException("StoreImage tapılmadı!");

            if (!string.IsNullOrWhiteSpace(img.Name))
                await _fileService.DeleteFile("store_images", img.Name);

            await _storeImageWriteRepo.SoftDeleteAsync(img);
            await _storeImageWriteRepo.CommitAsync();
        }
    }
}
