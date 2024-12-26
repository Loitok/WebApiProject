using AutoMapper;
using BLL.Models;
using BLL.Models.Result;
using BLL.Models.Result.Generics;
using DAL.Entities;
using DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IResult<IReadOnlyCollection<ClientModel>>> GetAllClients()
        {
            try
            {
                var clients = await _clientRepository.GetAllWithQueryAsync(query =>
                    query.Include(c => c.Locations.Take(1))
                    .AsNoTracking());

                var result = _mapper.Map<List<ClientModel>>(clients);

                return Result<IReadOnlyCollection<ClientModel>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyCollection<ClientModel>>.CreateFailure("Get Clients error!", ex);
            }
        }

        public async Task<IResult<IReadOnlyCollection<LocationModel>>> GetLocations()
        {
            try
            {
                var locations = await GetLocationsAsync();

                var result = _mapper.Map<IReadOnlyCollection<LocationModel>>(locations);

                return Result<IReadOnlyCollection<LocationModel>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyCollection<LocationModel>>.CreateFailure("Get Locations error!", ex);
            }
        }

        public async Task<IResult<byte[]>> GetExportedLocations()
        {
            try
            {
                var locations = await GetLocationsAsync();

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add(TableName);

                var properties = typeof(LocationModel)
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

        //public async Task<IResult> ImportLocations(IFormFile file)
        //{
        //    try
        //    {
        //        using var stream = new MemoryStream();

        //        await file.CopyToAsync(stream);

        //        using var package = new ExcelPackage(stream);

        //        var worksheet = package.Workbook.Worksheets[0];
        //        int rowCount = worksheet.Dimension.Rows;


        //        var excelLocationIds = new HashSet<int>();

        //        for (int row = FirstValueRow + 1; row <= rowCount; row++)
        //        {
        //            var locationId = Convert.ToInt32(worksheet.Cells[row, 1].Text);
        //            excelLocationIds.Add(locationId);
        //        }

        //        var dbLocationIds = await _locationRepository.GetSelectedAsync(location => location.Id); 

        //        var missingFromExcel = dbLocationIds.Except(excelLocationIds).ToList();

        //        if (missingFromExcel.Any())
        //            return Result.CreateFailure($"The following locations are missing from the Excel file: {string.Join(", ", missingFromExcel)}");

        //        var locationsToUpdate = new List<LocationEntity>();

        //        for (int row = FirstValueRow + 1; row <= rowCount; row++)
        //        {
        //            var location = new LocationModel
        //            {
        //                Id = Convert.ToInt32(worksheet.Cells[row, 1].Text),
        //                Address1 = worksheet.Cells[row, 2].Text,
        //                Address2 = worksheet.Cells[row, 3].Text,
        //                Address3 = worksheet.Cells[row, 4].Text,
        //                Attention = worksheet.Cells[row, 5].Text,
        //                BusinessId = worksheet.Cells[row, 6].Text,
        //                City = worksheet.Cells[row, 7].Text,
        //                ClientLocationNumber = worksheet.Cells[row, 8].Text,
        //                Country = worksheet.Cells[row, 9].Text,
        //                CreatedAt = Convert.ToDateTime(worksheet.Cells[row, 10].Text),
        //                D365LocationNumber = worksheet.Cells[row, 11].Text,
        //                Email = worksheet.Cells[row, 12].Text,
        //                IsArchived = Convert.ToBoolean(worksheet.Cells[row, 13].Text),
        //                Name = worksheet.Cells[row, 14].Text,
        //                Name2 = worksheet.Cells[row, 15].Text,
        //                Notes = worksheet.Cells[row, 16].Text,
        //                PhoneNumber = worksheet.Cells[row, 17].Text,
        //                PostalCode = worksheet.Cells[row, 18].Text,
        //                ProvinceOrState = worksheet.Cells[row, 19].Text,
        //                Status = worksheet.Cells[row, 20].Text,
        //                UpdatedAt = Convert.ToDateTime(worksheet.Cells[row, 21].Text)
        //            };

        //            var existingLocations = await _locationRepository.GetAllWithQueryAsync(locations =>
        //                locations.Where(x => x.Name == location.Name));

        //            var hasMoreId = existingLocations.Any(x => x.Id != location.Id);

        //            if (hasMoreId)
        //                return Result.CreateFailure($"Location with Name '{location.Name}' already exists.");

        //            var validationResults = new List<ValidationResult>();
        //            var isValid = Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true);

        //            if (!isValid)
        //                return Result.CreateFailure($"Validation failed for row {row}: {string.Join(", ", validationResults.Select(vr => vr.ErrorMessage))}");

        //            var locationInDB = await _locationRepository.GetByFirstOrDefaultAsync(locations =>
        //                locations.Where(x => x.Id == location.Id));

        //            if (locationInDB is not null)
        //            {
        //                locationInDB.Address1 = location.Address1;
        //                locationInDB.Address2 = location.Address2;
        //                locationInDB.Address3 = location.Address3;
        //                locationInDB.Attention = location.Attention;
        //                locationInDB.BusinessId = location.BusinessId;
        //                locationInDB.City = location.City;
        //                locationInDB.ClientLocationNumber = location.ClientLocationNumber;
        //                locationInDB.Country = location.Country;
        //                locationInDB.D365LocationNumber = location.D365LocationNumber;
        //                locationInDB.Email = location.Email;
        //                locationInDB.IsArchived = location.IsArchived;
        //                locationInDB.Name = location.Name;
        //                locationInDB.Name2 = location.Name2;
        //                locationInDB.Notes = location.Notes;
        //                locationInDB.PhoneNumber = location.PhoneNumber;
        //                locationInDB.PostalCode = location.PostalCode;
        //                locationInDB.ProvinceOrState = location.ProvinceOrState;
        //                locationInDB.Status = location.Status;
        //                locationInDB.UpdatedAt = DateTime.Now;

        //                locationsToUpdate.Add(locationInDB);
        //            }
        //            else
        //            {
        //                return Result.CreateFailure($"Location with ID {location.Id} not found.");
        //            }
        //        }

        //        if (locationsToUpdate.Any())
        //        {
        //            _locationRepository.UpdateRange(locationsToUpdate);
        //            await _locationRepository.SaveChangesAsync();
        //        }

        //        return Result.CreateSuccess();
        //    }
        //    catch (Exception ex) 
        //    {
        //        return Result.CreateFailure("Import Locations error!", ex);
        //    }
        //}

        public async Task<IResult> ImportLocations(IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                var excelLocationIds = new HashSet<int>();
                for (int row = FirstValueRow + 1; row <= rowCount; row++)
                {
                    var locationId = Convert.ToInt32(worksheet.Cells[row, 1].Text);
                    excelLocationIds.Add(locationId);
                }

                var dbLocationIds = await _locationRepository.GetSelectedAsync(location => location.Id);
                var missingFromExcel = dbLocationIds.Except(excelLocationIds).ToList();
                if (missingFromExcel.Any())
                    return Result.CreateFailure($"The following locations are missing from the Excel file: {string.Join(", ", missingFromExcel)}");

                var semaphore = new SemaphoreSlim(10, 10);
                var messages = new ConcurrentBag<string>();
                var locationsToUpdate = new ConcurrentBag<LocationEntity>();
                var tasks = new List<Task>();

                for (int row = FirstValueRow + 1; row <= rowCount; row++)
                {
                    await semaphore.WaitAsync();
                    var currentRow = row;

                    tasks.Add(Task.Run(async () => 
                    {
                        try
                        {
                            messages.Add($"Thread {Thread.CurrentThread.ManagedThreadId}: processing the row {currentRow} started.");

                            var location = new LocationModel
                            {
                                Id = Convert.ToInt32(worksheet.Cells[currentRow, 1].Text),
                                Address1 = worksheet.Cells[currentRow, 2].Text,
                                Address2 = worksheet.Cells[currentRow, 3].Text,
                                Address3 = worksheet.Cells[currentRow, 4].Text,
                                Attention = worksheet.Cells[currentRow, 5].Text,
                                BusinessId = worksheet.Cells[currentRow, 6].Text,
                                City = worksheet.Cells[currentRow, 7].Text,
                                ClientLocationNumber = worksheet.Cells[currentRow, 8].Text,
                                Country = worksheet.Cells[currentRow, 9].Text,
                                CreatedAt = Convert.ToDateTime(worksheet.Cells[currentRow, 10].Text),
                                D365LocationNumber = worksheet.Cells[currentRow, 11].Text,
                                Email = worksheet.Cells[currentRow, 12].Text,
                                IsArchived = Convert.ToBoolean(worksheet.Cells[currentRow, 13].Text),
                                Name = worksheet.Cells[currentRow, 14].Text,
                                Name2 = worksheet.Cells[currentRow, 15].Text,
                                Notes = worksheet.Cells[currentRow, 16].Text,
                                PhoneNumber = worksheet.Cells[currentRow, 17].Text,
                                PostalCode = worksheet.Cells[currentRow, 18].Text,
                                ProvinceOrState = worksheet.Cells[currentRow, 19].Text,
                                Status = worksheet.Cells[currentRow, 20].Text,
                                UpdatedAt = Convert.ToDateTime(worksheet.Cells[currentRow, 21].Text)
                            };

                            var validationResults = new List<ValidationResult>();

                            var isValid = Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true);
                            if (!isValid)
                                messages.Add($"Thread {Thread.CurrentThread.ManagedThreadId}: validation failed for row {currentRow}: {string.Join(", ", validationResults.Select(vr => vr.ErrorMessage))}");

                            var existingLocations = await _locationRepository.GetAllWithQueryAsync(locations =>
                                locations.Where(x => x.Name == location.Name));

                            var hasMoreId = existingLocations.Any(x => x.Id != location.Id);
                            if (hasMoreId)
                                messages.Add($"Thread {Thread.CurrentThread.ManagedThreadId}: location with Name {location.Name} already exists.");

                            var locationInDB = await _locationRepository.GetByFirstOrDefaultAsync(locations =>
                                locations.Where(x => x.Id == location.Id));

                            if (locationInDB != null)
                            {
                                locationInDB.Address1 = location.Address1;
                                locationInDB.Address2 = location.Address2;
                                locationInDB.Address3 = location.Address3;
                                locationInDB.Attention = location.Attention;
                                locationInDB.BusinessId = location.BusinessId;
                                locationInDB.City = location.City;
                                locationInDB.ClientLocationNumber = location.ClientLocationNumber;
                                locationInDB.Country = location.Country;
                                locationInDB.D365LocationNumber = location.D365LocationNumber;
                                locationInDB.Email = location.Email;
                                locationInDB.IsArchived = location.IsArchived;
                                locationInDB.Name = location.Name;
                                locationInDB.Name2 = location.Name2;
                                locationInDB.Notes = location.Notes;
                                locationInDB.PhoneNumber = location.PhoneNumber;
                                locationInDB.PostalCode = location.PostalCode;
                                locationInDB.ProvinceOrState = location.ProvinceOrState;
                                locationInDB.Status = location.Status;
                                locationInDB.UpdatedAt = DateTime.Now;

                                locationsToUpdate.Add(locationInDB);
                                messages.Add($"Thread {Thread.CurrentThread.ManagedThreadId}: successfully updated ID {location.Id}.");
                            }
                            else
                            {
                                messages.Add($"Thread {Thread.CurrentThread.ManagedThreadId}: ID {location.Id} wasn't found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            messages.Add($"Thread {Thread.CurrentThread.ManagedThreadId}: error in {currentRow}: {ex.Message}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }

                await Task.WhenAll(tasks);

                if (locationsToUpdate.Any())
                {
                    _locationRepository.UpdateRange(locationsToUpdate);
                    await _locationRepository.SaveChangesAsync();
                }

                foreach (var message in messages)
                {
                    Console.WriteLine(message);
                }

                return Result.CreateSuccess();
            }
            catch (Exception ex)
            {
                return Result.CreateFailure("Import Locations error!", ex);
            }
        }

        private async Task<IEnumerable<LocationModel>> GetLocationsAsync()
        {
            var locations = await _locationRepository.GetAllAsync();

            var result = _mapper.Map<IReadOnlyCollection<LocationModel>>(locations);
            
            return result;
        }

        private void SetupWorksheet(ExcelWorksheet worksheet, IList<string> properties)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                worksheet.Cells[FirstValueRow, i + 1].Value = properties[i];
            }
        }

        private void FillWorksheet(ExcelWorksheet worksheet, IEnumerable<LocationModel> locations, IList<string> properties)
        {
            var row = FirstValueRow + 1;

            foreach (var location in locations)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    var property = location.GetType().GetProperty(properties[col]);

                    var value = property?.GetValue(location) is DateTime date ? 
                        date.ToString("yyyy-MM-dd HH:mm:ss") :
                        property?.GetValue(location);

                    worksheet.Cells[row, col + 1].Value = value;
                }
                row++;
            }
        }
    }
}
