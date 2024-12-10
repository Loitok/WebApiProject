using DAL.Entities;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<ClientEntity> _clientRepository;
        private readonly IRepository<LocationEntity> _locationRepository;

        public ClientService(IRepository<ClientEntity> clientRepository, IRepository<LocationEntity> locationRepository)
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
        }

        public async Task<IReadOnlyCollection<ClientEntity>> GetAllClients()
        {
            try
            {
                var allClients = await _clientRepository
                    .GetAllWithIncludeAsync(query => query.Include(c => c.Locations));

                // TODO use mapper

                return allClients.ToList();
            }
            catch (Exception ex)
            {
                return new List<ClientEntity>();
            }
        }

        public async Task<IReadOnlyCollection<LocationEntity>> GetAllLocations()
        {
            try
            {
                var allLocations = await _locationRepository
                    .GetAllWithIncludeAsync(query => query.Include(c => c.Client));

                // TODO use mapper

                return allLocations.ToList();
            }
            catch (Exception ex)
            {
                return new List<LocationEntity>();
            }
        }
    }
}
