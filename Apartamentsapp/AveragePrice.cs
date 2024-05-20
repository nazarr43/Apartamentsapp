using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartamentsapp
{
    public class AveragePrice
    {
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);
        Dictionary<string, Flat> flatsDictionary;
        private readonly ILogger _logger;
        public AveragePrice(Dictionary<string, Flat> flatsDictionary, ILogger logger)
        {
            this.flatsDictionary = flatsDictionary;
            _logger = logger;
        }
        public async Task GetAveragePriceAsync(District district)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var pricesInDistrict = flatsDictionary.Values
                    .Where(flat => flat.District == district)
                    .Select(flat => flat.ApartmentPrice);
                if (pricesInDistrict.Any())
                {
                    double AveragePrice = pricesInDistrict.Average();
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
                var allPrices = flatsDictionary.Values.Select(flat => flat.Area);
                if (allPrices.Any())
                {
                    double averagePrice = allPrices.Average();
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
