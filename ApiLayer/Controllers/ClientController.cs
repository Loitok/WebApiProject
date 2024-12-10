using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Route("clients")]
        public async Task<IActionResult> GetClients()
        {
            var result = await _clientService.GetAllClients();
            return Ok(result);
        }

        [HttpGet]
        [Route("locations")]
        public async Task<IActionResult> GetLocations()
        {
            var result = await _clientService.GetAllLocations();
            return Ok(result);
        }
    }
}
