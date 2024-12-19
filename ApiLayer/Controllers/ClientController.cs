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

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage.Message);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("locations")]
        public async Task<IActionResult> GetLocations(int pageNumber, int pageSize)
        {
            var result = await _clientService.GetLocations(pageNumber, pageSize);
            
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage.Message);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("export-locations")]
        public async Task<IActionResult> GetExportedLocation(int pageNumber, int pageSize)
        {
            var result = await _clientService.GetExportedLocations(pageNumber, pageSize);

            if (!result.Success) {
                return BadRequest(result.ErrorMessage.Message);
            }

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Locations_Page.xlsx");
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
