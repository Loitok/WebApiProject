using ApiLayer.DTOs;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IDataSeeder dataSeeder, IMapper mapper)
        {
            _clientService = clientService;
            _dataSeeder = dataSeeder;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("clients")]
        public async Task<IActionResult> GetClients()
        {
            var clientsResult = await _clientService.GetAllClients();

            if (!clientsResult.Success)
                return BadRequest(clientsResult.ErrorMessage.Message);

            var result = _mapper.Map<List<ClientDTO>>(clientsResult.Data);

            return Ok(result);
        }

        [HttpGet]
        [Route("locations")]
        public async Task<IActionResult> GetLocations()
        {
            var locationsResult = await _clientService.GetLocations();

            if (!locationsResult.Success)
                return BadRequest(locationsResult.ErrorMessage.Message);

            var result = _mapper.Map<List<LocationDTO>>(locationsResult.Data);

            return Ok(result);
        }

        [HttpGet]
        [Route("export-locations")]
        public async Task<IActionResult> GetExportedLocation()
        {
            var result = await _clientService.GetExportedLocations();

            if (!result.Success)
                return BadRequest(result.ErrorMessage.Message);

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Locations_Page.xlsx");
        }

        [HttpPost]
        [Route("import-locations")]
        public async Task<IActionResult> ImportLocations(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var result = await _clientService.ImportLocations(file);

            if (!result.Success)
                return BadRequest(result.ErrorMessage.Message);

            return Ok(result.Success);
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
