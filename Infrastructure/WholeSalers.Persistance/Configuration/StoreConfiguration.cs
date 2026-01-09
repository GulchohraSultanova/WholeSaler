using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Configuration
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.SubTitle)
                   .IsRequired()
                   ;

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

            // Store -> WholeSaler (Many to One)
            builder.HasOne(x => x.WholeSaler)
                   .WithMany(x => x.Stores)
                   .HasForeignKey(x => x.WholeSalerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Store -> StoreImages (One to Many)
            builder.HasMany(x => x.StoreImages)
                   .WithOne(x => x.Store)
                   .HasForeignKey(x => x.StoreId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Store -> Category (Many to One)
            builder.HasOne(x => x.Category)
                  .WithMany(x => x.Stores)
                  .HasForeignKey(x => x.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
