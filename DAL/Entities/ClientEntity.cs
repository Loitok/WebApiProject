using DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [EntityTypeConfiguration(typeof(ClientConfiguration))]
    public class ClientEntity : BaseEntity
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ICollection<LocationEntity> Locations { get; set; } = new List<LocationEntity>();
    }
}
