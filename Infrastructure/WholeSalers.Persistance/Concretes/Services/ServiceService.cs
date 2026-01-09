using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Services;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Service;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceReadRepository _serviceReadRepo;
        private readonly IServiceWriteRepository _serviceWriteRepo;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ServiceService(
            IServiceReadRepository serviceReadRepo,
            IServiceWriteRepository serviceWriteRepo,
            IFileService fileService,
            IMapper mapper)
        {
            _serviceReadRepo = serviceReadRepo;
            _serviceWriteRepo = serviceWriteRepo;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<ServiceDto>> GetAllAsync()
        {
            // Repo-da EnableTraking default false -> AsNoTracking avtomatik olur
            var services = await _serviceReadRepo.GetAllAsync(s => !s.IsDeleted);

            return _mapper.Map<List<ServiceDto>>(services.ToList());
        }

        public async Task<ServiceDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Service ID!");

            // GetByIdAsync(string) səndə GUID üçündür, ona görə GetAsync istifadə edirik
            var service = await _serviceReadRepo.GetAsync(s => s.Id == id && !s.IsDeleted);

            if (service == null)
                throw new GlobalAppException("Service tapılmadı!");

            return _mapper.Map<ServiceDto>(service);
        }

        public async Task CreateAsync(CreateServiceDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            var service = _mapper.Map<Service>(dto);

            if (dto.CardImage != null)
                service.CardImage = await _fileService.UploadFile(dto.CardImage, "services");

            await _serviceWriteRepo.AddAsync(service);
            await _serviceWriteRepo.CommitAsync();
        }

        public async Task UpdateAsync(UpdateServiceDto dto)
        {
            if (dto == null || dto.Id <= 0 )
                throw new GlobalAppException("Yanlış Service ID!");

            var service = await _serviceReadRepo.GetAsync(s => s.Id == dto.Id && !s.IsDeleted)
                          ?? throw new GlobalAppException("Service tapılmadı!");

            var oldTitle = service.Title;
            var oldSubTitle = service.SubTitle;

            _mapper.Map(dto, service);

            if (dto.Title == null) service.Title = oldTitle;
            if (dto.SubTitle == null) service.SubTitle = oldSubTitle;

            if (dto.CardImage != null)
            {
                if (!string.IsNullOrWhiteSpace(service.CardImage))
                    await _fileService.DeleteFile("services", service.CardImage);

                service.CardImage = await _fileService.UploadFile(dto.CardImage, "services");
            }

            await _serviceWriteRepo.UpdateAsync(service);
            await _serviceWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Service ID!");

            // GUID repo metodu yox, int ID ilə GetAsync
            var service = await _serviceReadRepo.GetAsync(s => s.Id == id && !s.IsDeleted)
                          ?? throw new GlobalAppException("Service tapılmadı!");

            if (!string.IsNullOrWhiteSpace(service.CardImage))
                await _fileService.DeleteFile("services", service.CardImage);

            await _serviceWriteRepo.SoftDeleteAsync(service);
            await _serviceWriteRepo.CommitAsync();
        }
    }
}
