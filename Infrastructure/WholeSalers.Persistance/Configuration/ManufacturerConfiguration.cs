using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Configuration
{
    public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.ToTable("Manufacturers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(x => x.CardImage)
                   .IsRequired()
                   .HasMaxLength(500);
            builder.Property(x => x.MobilePhone)
                 .HasMaxLength(20);

            builder.Property(x => x.WhatsappPhone)
                   .HasMaxLength(20);

            builder.Property(x => x.TikTokLink)
                   .HasMaxLength(500);

            builder.Property(x => x.WebSiteUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.InstaLink)
                   .HasMaxLength(500);

            builder.Property(x => x.InLocation)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(x => x.MapLocation)
                   .HasMaxLength(1000);

            builder.Property(x => x.YoutubeLink)
                   .HasMaxLength(500);
            // Store -> StoreImages (One to Many)
            builder.HasMany(x => x.ManufacturerImages)
                   .WithOne(x => x.Manufacturer)
                   .HasForeignKey(x => x.ManufacturerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
