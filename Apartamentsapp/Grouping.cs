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
        Dictionary<string, Flats> flatsDictionary;
        public Grouping(Dictionary<string, Flats> flatsDictionary)
        {
            this.flatsDictionary = flatsDictionary;
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
