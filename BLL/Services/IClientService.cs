using BLL.DTOs;
using BLL.DTOs.Result;

namespace BLL.Services
{
    public interface IClientService
    {
        Task<IResult<IReadOnlyCollection<ClientDTO>>> GetAllClients();
        Task<IResult<IReadOnlyCollection<LocationDTO>>> GetLocations(int pageNumber, int pageSize);
        Task<IResult<byte[]>> GetExportedLocations(int pageNumber, int pageSize);
    }
}
