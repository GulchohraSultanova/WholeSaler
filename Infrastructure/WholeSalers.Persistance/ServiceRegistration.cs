using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories.Categories;
using WholeSalers.Application.Abstracts.Repositories.Contacts;
using WholeSalers.Application.Abstracts.Repositories.ManufacturerImages;
using WholeSalers.Application.Abstracts.Repositories.Manufacturers;
using WholeSalers.Application.Abstracts.Repositories.Services;
using WholeSalers.Application.Abstracts.Repositories.Sliders;
using WholeSalers.Application.Abstracts.Repositories.StoreImages;
using WholeSalers.Application.Abstracts.Repositories.Stores;
using WholeSalers.Application.Abstracts.Repositories.WholeSalers;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Infrastructure.Concretes;
using WholeSalers.Persistance.Concretes.Repositories.Categories;
using WholeSalers.Persistance.Concretes.Repositories.Contacts;
using WholeSalers.Persistance.Concretes.Repositories.ManufacturerImages;
using WholeSalers.Persistance.Concretes.Repositories.Manufacturers;
using WholeSalers.Persistance.Concretes.Repositories.Services;
using WholeSalers.Persistance.Concretes.Repositories.Sliders;
using WholeSalers.Persistance.Concretes.Repositories.StoreImages;
using WholeSalers.Persistance.Concretes.Repositories.Stores;
using WholeSalers.Persistance.Concretes.Repositories.WholeSalers;
using WholeSalers.Persistance.Concretes.Services;

namespace WholeSalers.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IServiceReadRepository, ServiceReadRepository>();
            services.AddScoped<IServiceWriteRepository, ServiceWriteRepository>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<IManufacturerReadRepository, ManufacturerReadRepository>();
            services.AddScoped<IManufacturerWriteRepository, ManufacturerWriteRepository>();
            services.AddScoped<IMailService,MailService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactReadRepository, ContactReadRepository>();
            services.AddScoped<IContactWriteRepository, ContactWriteRepository>();

            services.AddScoped<IWholeSalerReadRepository, WholeSalerReadRepository>();
            services.AddScoped<IWholeSalerWriteRepository, WholeSalerWriteRepository>();
            services.AddScoped<IWholeSalerService, WholeSalerService>();

            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreReadRepository,StoreReadRepository>();
            services.AddScoped<IStoreWriteRepository,StoreWriteRepository>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ISliderReadRepository, SliderReadRepository>();
            services.AddScoped<ISliderWriteRepository, SliderWriteRepository>();

            services.AddScoped<IStoreImageService, StoreImageService>();
            services.AddScoped<IStoreImageReadRepository, StoreImageReadRepository>();
            services.AddScoped<IStoreImageWriteRepository, StoreImageWriteRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
         services.AddScoped<IManufacturerImageReadRepository, ManufacturerImageReadRepository>();
            services.AddScoped<IManufacturerImageWriteRepository, ManufacturerImageWriteRepository>();
            services.AddScoped<IManufacturerImageService, ManufacturerImageService>();

        }
    }
}
