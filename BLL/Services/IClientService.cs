using DAL.Entities;

namespace BLL.Services
{
    public interface IClientService
    {
        Task<IReadOnlyCollection<ClientEntity>> GetAllClients();
        Task<IReadOnlyCollection<LocationEntity>> GetAllLocations();
    }
}
