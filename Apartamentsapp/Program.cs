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
    Dictionary<string, Flat> flats = new Dictionary<string, Flat>();

    DataLoader.ReadDataFromCSV(filePath, flats);
    var districts = flats.Values
                    .Select(flat => flat.District)
                    .Distinct()
                    .ToArray();
    List<Task> tasks = new List<Task>();
    var grouping = new Grouping(flats, Log.Logger);
    var averagePriceCalculator = new AveragePrice(flats, Log.Logger);
    foreach (var district in districts)
    {
        tasks.Add(grouping.GetGroupedAndSorted(district));
        tasks.Add(averagePriceCalculator.GetAveragePriceAsync(district));
    }

    tasks.Add(averagePriceCalculator.GetAveragePricesForCitiesAsync());
    await Task.WhenAll(tasks);
    Console.WriteLine("All tasks completed.");
}
catch (Exception ex)
{
    Log.Information($"An error occurred: {ex.Message}");
}
