namespace BLL.Models
{
    public class ClientModel : BaseModel
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ICollection<LocationModel> Locations { get; set; }
    }
}
