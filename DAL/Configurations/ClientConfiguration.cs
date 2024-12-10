using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Locations)
                .WithOne(x => x.Client)
                .HasForeignKey(x => x.ClientId)
                .IsRequired();
        }
    }
}
