namespace BLL.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ICollection<LocationDto> Locations { get; }
    }
}
