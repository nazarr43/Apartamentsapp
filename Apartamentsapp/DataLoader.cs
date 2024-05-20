namespace Apartamentsapp
{
    public class DataLoader
    {
        public static void ReadDataFromCSV(string filePath, Dictionary<string, Flat> flatsDictionary)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    reader.ReadLine();
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (values.Length != 4)
                        {
                            Console.WriteLine($"Incorrect data format in line: {line}");
                            continue;
                        }
                        try
                        {
                            flatsDictionary[values[0]] = new Flat { FlatName = values[0], ApartmentPrice = int.Parse(values[1]), Area = int.Parse(values[2]), District = values[3] };
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Incorrect data format in line: {line}");
                        }
                    }
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{filePath}' not found.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Incorrect data format in the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
