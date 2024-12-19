using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<LocationEntity>
    {
        public void Configure(EntityTypeBuilder<LocationEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClientId)
                .IsRequired();

            builder.HasIndex(x => x.Name);
        }
    }
}
