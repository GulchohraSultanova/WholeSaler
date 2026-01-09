using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Configuration
{
    public class StoreImageConfiguration : IEntityTypeConfiguration<StoreImage>
    {
        public void Configure(EntityTypeBuilder<StoreImage> builder)
        {
            builder.ToTable("StoreImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(x => x.Store)
                   .WithMany(x => x.StoreImages)
                   .HasForeignKey(x => x.StoreId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
