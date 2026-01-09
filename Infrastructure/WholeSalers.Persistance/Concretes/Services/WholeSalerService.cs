using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.WholeSalers;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.WholeSaler;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class WholeSalerService : IWholeSalerService
    {
        private readonly IWholeSalerReadRepository _wholeSalerReadRepo;
        private readonly IWholeSalerWriteRepository _wholeSalerWriteRepo;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public WholeSalerService(
            IWholeSalerReadRepository wholeSalerReadRepo,
            IWholeSalerWriteRepository wholeSalerWriteRepo,
            IFileService fileService,
            IMapper mapper)
        {
            _wholeSalerReadRepo = wholeSalerReadRepo;
            _wholeSalerWriteRepo = wholeSalerWriteRepo;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<WholeSalerDto>> GetAllAsync()
        {
            var list = await _wholeSalerReadRepo.GetAllAsync(
                w => !w.IsDeleted,
                include: q => q.Include(x => x.Stores).ThenInclude(x=>x.StoreImages)
            );

            return _mapper.Map<List<WholeSalerDto>>(list.ToList());
        }

        public async Task<WholeSalerDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış WholeSaler ID!");

            var wholesaler = await _wholeSalerReadRepo.GetAsync(
                w => w.Id == id && !w.IsDeleted,
                include: q => q.Include(x => x.Stores).ThenInclude(x => x.StoreImages)
            );

            if (wholesaler == null)
                throw new GlobalAppException("WholeSaler tapılmadı!");

            return _mapper.Map<WholeSalerDto>(wholesaler);
        }

        public async Task CreateAsync(CreateWholeSalerDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            var wholesaler = _mapper.Map<WholeSaler>(dto);

            if (dto.CardImage != null)
                wholesaler.CardImage = await _fileService.UploadFile(dto.CardImage, "wholesalers");

            await _wholeSalerWriteRepo.AddAsync(wholesaler);
            await _wholeSalerWriteRepo.CommitAsync();
        }

        public async Task UpdateAsync(UpdateWholeSalerDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new GlobalAppException("Yanlış WholeSaler ID!");

            var wholesaler = await _wholeSalerReadRepo.GetAsync(w => w.Id == dto.Id && !w.IsDeleted)
                             ?? throw new GlobalAppException("WholeSaler tapılmadı!");

            // keep old values if null comes
            var oldTitle = wholesaler.Title;
            var oldSubTitle = wholesaler.SubTitle;
            var oldLocation = wholesaler.Location;

            _mapper.Map(dto, wholesaler);

            if (dto.Title == null) wholesaler.Title = oldTitle;
            if (dto.SubTitle == null) wholesaler.SubTitle = oldSubTitle;
            if (dto.Location == null) wholesaler.Location = oldLocation;

            if (dto.CardImage != null)
            {
                if (!string.IsNullOrWhiteSpace(wholesaler.CardImage))
                    await _fileService.DeleteFile("wholesalers", wholesaler.CardImage);

                wholesaler.CardImage = await _fileService.UploadFile(dto.CardImage, "wholesalers");
            }

            await _wholeSalerWriteRepo.UpdateAsync(wholesaler);
            await _wholeSalerWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış WholeSaler ID!");

            var wholesaler = await _wholeSalerReadRepo.GetAsync(w => w.Id == id && !w.IsDeleted)
                             ?? throw new GlobalAppException("WholeSaler tapılmadı!");

            if (!string.IsNullOrWhiteSpace(wholesaler.CardImage))
                await _fileService.DeleteFile("wholesalers", wholesaler.CardImage);

            await _wholeSalerWriteRepo.SoftDeleteAsync(wholesaler);
            await _wholeSalerWriteRepo.CommitAsync();
        }
    }
}
