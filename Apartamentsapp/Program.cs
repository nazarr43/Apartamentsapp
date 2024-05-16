using Apartamentsapp;
try
{
    Console.WriteLine("Enter the path to the CSV file:");
string filePath = Console.ReadLine();

if (!File.Exists(filePath))
{
    Console.WriteLine($"File '{filePath}' not found.");
    return;
}
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

tasks.Add(averagePriceCalculator.GetAveragePriceForCityAsync());
await Task.WhenAll(tasks);
Console.WriteLine("All tasks completed.");
}
catch (Exception ex)
            {
    Console.WriteLine($"An error occurred: {ex.Message}");
}
