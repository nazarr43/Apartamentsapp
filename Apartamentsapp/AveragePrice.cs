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
        Dictionary<string, Flats> flatsDictionary;
        public AveragePrice(Dictionary<string, Flats> flatsDictionary)
        {
            this.flatsDictionary = flatsDictionary;
        }
        public async Task GetAveragePriceAsync(string District)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var pricesInDistrict = flatsDictionary.Values
                    .Where(flat => flat.District == District)
                    .Select(flat => flat.Area);
                if (pricesInDistrict.Any())
                {
                    double AveragePrice = pricesInDistrict.Average();
                    Console.WriteLine($"Average price in {District}: {AveragePrice}");
                }
                else
                {
                    Console.WriteLine($"No flats found in {District}");
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
