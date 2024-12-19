using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.Result;
using DAL.Entities;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace BLL.Services
{
    public class ClientService : IClientService
    {
        private const int FirstValueRow = 1;
        private const string TableName = "Locations";

        private readonly IRepository<ClientEntity> _clientRepository;
        private readonly IRepository<LocationEntity> _locationRepository;
        private readonly IMapper _mapper;

        public ClientService(
            IRepository<ClientEntity> clientRepository,
            IRepository<LocationEntity> locationRepository,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<IResult<IReadOnlyCollection<ClientDTO>>> GetAllClients()
        {
            try
            {
                var allClients = await _clientRepository.GetAllWithIncludeAsync(query =>
                    query.Include(c => c.Locations.Take(1))
                    .AsNoTracking());

                var result = _mapper.Map<List<ClientDTO>>(allClients);

                return Result<IReadOnlyCollection<ClientDTO>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyCollection<ClientDTO>>.CreateFailure("Get Clients error!", ex);
            }
        }

        public async Task<IResult<IReadOnlyCollection<LocationDTO>>> GetLocations(int pageNumber, int pageSize)
        {
            try
            {
                var locations = await GetLocationsAsync(pageNumber, pageSize);

                var result = _mapper.Map<IReadOnlyCollection<LocationDTO>>(locations);

                return Result<IReadOnlyCollection<LocationDTO>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyCollection<LocationDTO>>.CreateFailure("Get Locations error!", ex);
            }
        }

        public async Task<IResult<byte[]>> GetExportedLocations(int pageNumber, int pageSize)
        {
            try
            {
                var locations = await GetLocationsAsync(pageNumber, pageSize);

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add(TableName);

                var properties = typeof(LocationDTO)
                    .GetProperties()
                    .Select(p => p.Name)
                    .OrderBy(name => name == "Id" ? 0 : 1)
                    .ThenBy(name => name)
                    .ToList();

                SetupWorksheet(worksheet, properties);

                FillWorksheet(worksheet, locations, properties);

                worksheet.Cells.AutoFitColumns();

                return Result<byte[]>.CreateSuccess(package.GetAsByteArray());
            }
            catch (Exception ex)
            {
                return Result<byte[]>.CreateFailure("Export Locations error!", ex);
            }
        }

        private async Task<IEnumerable<LocationEntity>> GetLocationsAsync(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;

            return await _locationRepository
                .GetAllWithIncludeAsync(query => query
                .Include(c => c.Client)
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking());
        }

        private void SetupWorksheet(ExcelWorksheet worksheet, IList<string> properties)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                worksheet.Cells[FirstValueRow, i + 1].Value = properties[i];
            }
        }

        private void FillWorksheet(ExcelWorksheet worksheet, IEnumerable<LocationEntity> locations, IList<string> properties)
        {
            var row = FirstValueRow + 1;

            foreach (var location in locations)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    var property = location.GetType().GetProperty(properties[col]);

                    var value = property?.Name is "Client" && location.Client is not null ? location.Client.Name :
                        property?.GetValue(location) is DateTime date ? date.ToString("yyyy-MM-dd HH:mm:ss") :
                        property?.GetValue(location);

                    worksheet.Cells[row, col + 1].Value = value;
                }
                row++;
            }
        }
    }
}
