using BLL.Models;
using BLL.Models.Result;
using BLL.Models.Result.Generics;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public interface IClientService
    {
        Task<IResult<IReadOnlyCollection<ClientModel>>> GetAllClients();
        Task<IResult<IReadOnlyCollection<LocationModel>>> GetLocations();
        Task<IResult<byte[]>> GetExportedLocations();
        Task<IResult> ImportLocations(IFormFile file);
    }
}
