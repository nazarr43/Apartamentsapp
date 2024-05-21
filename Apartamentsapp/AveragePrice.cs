using Serilog;

namespace Apartamentsapp
{
    public class AveragePrice
    {
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);
        Dictionary<string, Flat> _flats;
        private readonly ILogger _logger;
        public AveragePrice(Dictionary<string, Flat> flats, ILogger logger)
        {
            _flats = flats;
            _logger = logger;
        }
        public async Task GetAveragePriceAsync(District district)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var pricesInDistrict = _flats.Values
                    .Where(flat => flat.District == district)
                    .Select(flat => flat.ApartmentPrice);
                if (pricesInDistrict.Any())
                {
                    decimal AveragePrice = pricesInDistrict.Average();
                    Console.WriteLine($"Average price in {district}: {AveragePrice}");
                }
                else
                {
                    _logger.Information($"No flats found in {district}");
                }
            }
            catch (Exception ex)
            {
                _logger.Information($"An error occurred in GetAveragePriceAsync: {ex.Message}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        public async Task GetAveragePricesForCitiesAsync()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var allPrices = _flats.Values.Select(flat => flat.Area);
                if (allPrices.Any())
                {
                    decimal averagePrice = allPrices.Average();
                    Console.WriteLine($"Average price for the city: {averagePrice}");
                }
                else
                {
                    _logger.Information($"No flats found in the city");
                }
            }
            catch (Exception ex)
            {
                _logger.Information($"An error occurred in GetAveragePriceForCityAsync: {ex.Message}");

            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

    }
}
