using Apartamentsapp;


string filePath = @"D:\test.csv";
Dictionary<string, Flats> flatsDictionary = new Dictionary<string, Flats>();

DataLoader.ReadDataFromCSV(filePath, flatsDictionary);
var districts = flatsDictionary.Values
                .Select(flat => flat.District)
                .Distinct()
                .ToArray();
List<Task> tasks = new List<Task>();
Grouping grouping = new Grouping(flatsDictionary);
AveragePrice averagePriceCalculator = new AveragePrice(flatsDictionary);
foreach (var district in districts)
{
    tasks.Add(grouping.GroupingAndSortingAsync(district));
    tasks.Add(averagePriceCalculator.GetAveragePriceAsync(district));
}

await Task.WhenAll(tasks);

Console.WriteLine("All tasks completed.");

