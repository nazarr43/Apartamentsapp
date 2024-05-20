using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartamentsapp
{
    public class Grouping
    {
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);
        Dictionary<string, Flat> flatsDictionary;
        private readonly ILogger _logger;
        public Grouping(Dictionary<string, Flat> flatsDictionary, ILogger logger)
        {
            this.flatsDictionary = flatsDictionary;
            _logger = logger;
        }
        public async Task GroupingAndSortingAsync(string District)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var group = flatsDictionary.Values
                    .Where(flat => flat.District == District)
                    .OrderBy(flat => flat.FlatName);

                if (group.Any())
                {
                    Console.WriteLine($"District sort: {District}");
                    foreach (var flat in group)
                    {
                        Console.WriteLine($"Owner name: {flat.FlatName},ApartmentPrice: {flat.ApartmentPrice},Area: {flat.Area}");
                    }
                }
                else
                {
                    _logger.Information($"No flats found in {District}");
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
