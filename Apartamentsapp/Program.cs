using Apartamentsapp;


SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3); 
string filePath = @"D:\test.csv";

Dictionary<string, Flats> flatsDictionary = new Dictionary<string, Flats>();
DataLoader.ReadDataFromCSV(filePath, flatsDictionary);

Task task1 = GetAveragePrice("Lychakiv");
Task task2 = GetAveragePrice("Shevchenkivskyi");
Task task3 = GetAveragePrice("Sykhiv");
Task task4 = GetAveragePrice("Frankivskyi");
Task task5 = GetAveragePrice("Zaliznychnyi");

await Task.WhenAll(task1, task2, task3, task4, task5);
Console.WriteLine("All tasks completed.");

async Task GetAveragePrice(string District)
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