using BLL.Seeders;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IDataSeeder _dataSeeder;

        public ClientController(IClientService clientService, IDataSeeder dataSeeder)
        {
            _clientService = clientService;
            _dataSeeder = dataSeeder;
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

        [HttpPost]
        [Route("generate-locations")]
        public async Task<IActionResult> GenerateLocations(int number)
        {
            await _dataSeeder.SeedLocations(number);
            return Ok($"{number} new entities!");
        }
    }
}
