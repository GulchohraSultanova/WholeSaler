using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Configuration
{
    public class WholeSalerConfiguration : IEntityTypeConfiguration<WholeSaler>
    {
        public void Configure(EntityTypeBuilder<WholeSaler> builder)
        {
            builder.ToTable("WholeSalers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.SubTitle)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(x => x.CardImage)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Location)
                   .HasMaxLength(300);

            builder.HasMany(x => x.Stores)
                   .WithOne(x => x.WholeSaler)
                   .HasForeignKey(x => x.WholeSalerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
