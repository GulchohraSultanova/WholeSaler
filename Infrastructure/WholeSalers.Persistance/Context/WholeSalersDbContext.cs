using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Context
{
    // If you have your own user class, replace IdentityUser with AppUser
    public class WholeSalersDbContext : IdentityDbContext<Admin>
    {
        public WholeSalersDbContext(DbContextOptions<WholeSalersDbContext> options)
            : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreImage> StoreImages { get; set; }
        public DbSet<WholeSaler> WholeSalers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ManufacturerImage> ManufacturerImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Applies all IEntityTypeConfiguration<> classes in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(WholeSalersDbContext).Assembly);
        }
    }
}
