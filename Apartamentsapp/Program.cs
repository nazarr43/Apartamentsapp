using Apartamentsapp;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
Log.Information("Application starting...");
try
{
    Console.WriteLine("Enter the path to the CSV file:");
    string filePath = Console.ReadLine();

    if (!File.Exists(filePath))
    {
        Log.Information($"File '{filePath}' not found");
        return;
    }
    Dictionary<string, Flats> flatsDictionary = new Dictionary<string, Flats>();

    DataLoader.ReadDataFromCSV(filePath, flatsDictionary);
    var districts = flatsDictionary.Values
                    .Select(flat => flat.District)
                    .Distinct()
                    .ToArray();
    List<Task> tasks = new List<Task>();
    Grouping grouping = new Grouping(flatsDictionary, Log.Logger);
    AveragePrice averagePriceCalculator = new AveragePrice(flatsDictionary, Log.Logger);
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
    Log.Information($"An error occurred: {ex.Message}");
}
