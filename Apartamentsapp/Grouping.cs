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
        Dictionary<string, Flat> flats;
        private readonly ILogger _logger;
        public Grouping(Dictionary<string, Flat> flats, ILogger logger)
        {
            this.flats = flats;
            _logger = logger;
        }
        public async Task GetGroupedAndSorted(District district)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var group = flats.Values
                    .Where(flat => flat.District == district)
                    .OrderBy(flat => flat.FlatName);

                if (group.Any())
                {
                    Console.WriteLine($"District sort: {district}");
                    foreach (var flat in group)
                    {
                        Console.WriteLine($"Owner name: {flat.FlatName},ApartmentPrice: {flat.ApartmentPrice},Area: {flat.Area}");
                    }
                }
                else
                {
                    _logger.Information($"No flats found in {district}");
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
