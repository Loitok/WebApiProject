namespace BLL.DTOs
{
    public class ClientDTO : BaseDTO
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ICollection<LocationDTO> Locations { get; set; }
    }
}
