using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Categories;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Category;
using WholeSalers.Application.GlobalAppException;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Concretes.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryReadRepository _categoryReadRepo;
        private readonly ICategoryWriteRepository _categoryWriteRepo;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryReadRepository categoryReadRepo,
            ICategoryWriteRepository categoryWriteRepo,
            IFileService fileService,
            IMapper mapper)
        {
            _categoryReadRepo = categoryReadRepo;
            _categoryWriteRepo = categoryWriteRepo;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryReadRepo.GetAllAsync(
                c => !c.IsDeleted,
                include: q => q
                    .Include(c => c.Stores.Where(s => !s.IsDeleted))
            );

            return _mapper.Map<List<CategoryDto>>(categories.ToList());
        }

        public async Task<CategoryDto> GetByIdAsync(int? id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Category ID!");

            var category = await _categoryReadRepo.GetAsync(
                c => c.Id == id && !c.IsDeleted,
                include: q => q
                    .Include(c => c.Stores.Where(s => !s.IsDeleted))
            );

            if (category == null)
                throw new GlobalAppException("Category tapılmadı!");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task CreateAsync(CreateCategoryDto dto)
        {
            if (dto == null)
                throw new GlobalAppException("Göndərilən məlumat boşdur!");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new GlobalAppException("Name boş ola bilməz!");

            if (dto.Icon == null || dto.Icon.Length == 0)
                throw new GlobalAppException("Icon boş ola bilməz!");

            var category = new Category
            {
                Name = dto.Name.Trim()
            };

            // upload icon file -> save file name/path to entity.Icon
            category.Icon = await _fileService.UploadFile(dto.Icon, "category_icons");

            await _categoryWriteRepo.AddAsync(category);
            await _categoryWriteRepo.CommitAsync();
        }

        public async Task UpdateAsync(UpdateCategoryDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new GlobalAppException("Yanlış Category ID!");

            var category = await _categoryReadRepo.GetAsync(c => c.Id == dto.Id && !c.IsDeleted)
                           ?? throw new GlobalAppException("Category tapılmadı!");

            var oldName = category.Name;
            var oldIcon = category.Icon;

            // partial name update
            if (dto.Name != null)
                category.Name = dto.Name.Trim();
            else
                category.Name = oldName;

            // icon update
            if (dto.Icon != null && dto.Icon.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(oldIcon))
                    await _fileService.DeleteFile("category_icons", oldIcon);

                category.Icon = await _fileService.UploadFile(dto.Icon, "category_icons");
            }
            else
            {
                category.Icon = oldIcon;
            }

            await _categoryWriteRepo.UpdateAsync(category);
            await _categoryWriteRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new GlobalAppException("Yanlış Category ID!");

            var category = await _categoryReadRepo.GetAsync(c => c.Id == id && !c.IsDeleted)
                           ?? throw new GlobalAppException("Category tapılmadı!");

            // optional: delete icon file
            if (!string.IsNullOrWhiteSpace(category.Icon))
                await _fileService.DeleteFile("category_icons", category.Icon);

            await _categoryWriteRepo.SoftDeleteAsync(category);
            await _categoryWriteRepo.CommitAsync();
        }
    }
}
