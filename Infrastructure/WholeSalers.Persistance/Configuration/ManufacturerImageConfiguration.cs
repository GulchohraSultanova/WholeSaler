using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Persistance.Configuration
{
    public class ManufacturerImageConfiguration : IEntityTypeConfiguration<ManufacturerImage>
    {
        public void Configure(EntityTypeBuilder<ManufacturerImage> builder)
        {
            builder.ToTable("ManufacturerImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(x => x.Manufacturer)
                   .WithMany(x => x.ManufacturerImages)
                   .HasForeignKey(x => x.ManufacturerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
