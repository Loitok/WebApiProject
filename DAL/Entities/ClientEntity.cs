namespace DAL.Entities
{
    public class ClientEntity : BaseEntity
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ICollection<LocationEntity> Locations { get; set; } = new List<LocationEntity>();
    }
}
