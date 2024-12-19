using DAL;
using DAL.Entities;

namespace BLL.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public DataSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedLocations(int count)
        {
            var random = new Random();
            var locations = new List<LocationEntity>();
            var currentYear = DateTime.Now.Year;

            for (int i = 0; i < count; i++)
            {
                var location = new LocationEntity
                {
                    BusinessId = Guid.NewGuid().ToString(),
                    Name = $"Location-{i}",
                    Name2 = $"Secondary Name-{i}",
                    D365LocationNumber = $"D365-{random.Next(1000, 9999)}",
                    ClientLocationNumber = $"CLN-{random.Next(1000, 9999)}",
                    Address1 = $"Address Line 1-{i}",
                    Address2 = $"Address Line 2-{i}",
                    Address3 = $"Address Line 3-{i}",
                    City = $"City-{random.Next(1, 100)}",
                    ProvinceOrState = $"State-{random.Next(1, 50)}",
                    PostalCode = $"{random.Next(10000, 99999)}",
                    Country = "Country-X",
                    Attention = $"Person-{random.Next(1, 1000)}",
                    PhoneNumber = $"{random.Next(1000000000, 1999999999)}",
                    Email = $"email{i}@example.com",
                    Status = "Active",
                    Notes = $"Note-{i}",
                    IsArchived = random.Next(0, 2) == 1,
                    CreatedAt = GenerateRandomDate(currentYear, random),
                    UpdatedAt = GenerateRandomDate(currentYear, random),
                    ClientId = random.Next(1, 6)
                };

                locations.Add(location);

                if (locations.Count >= 1000)
                {
                    await _context.LocationEntities.AddRangeAsync(locations);
                    await _context.SaveChangesAsync();
                    locations.Clear();
                }
            }

            if (locations.Any())
            {
                await _context.LocationEntities.AddRangeAsync(locations);
                await _context.SaveChangesAsync();
            }
        }

        static DateTime GenerateRandomDate(int year, Random random)
        {
            int dayOfYear = random.Next(1, DateTime.IsLeapYear(year) ? 367 : 366);
            return new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
        }
    }
}
