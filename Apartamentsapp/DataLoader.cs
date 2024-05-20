namespace Apartamentsapp
{
    public class DataLoader
    {
        public static void ReadDataFromCSV(string filePath, Dictionary<string, Flat> flats)
        {
            List<string> districts = new List<string>();
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
                            if (string.IsNullOrEmpty(values[0]))
                            {
                                Console.WriteLine($"Flat name cannot be empty in line: {line}");
                                continue;
                            }
                            else if (!double.TryParse(values[1], out double apartmentPrice))
                            {
                                Console.WriteLine($"Invalid apartment price in line: {line}");
                                continue;
                            }
                            if (!double.TryParse(values[2], out double area))
                            {
                                Console.WriteLine($"Invalid area in line: {line}");
                                continue;
                            }

                            if (!Enum.TryParse(values[3], out District district))
                            {
                                Console.WriteLine($"Invalid district in line: {line}");
                                continue;
                            }
                            flats[values[0]] = new Flat { FlatName = values[0], ApartmentPrice = int.Parse(values[1]), Area = int.Parse(values[2]), District = (District)Enum.Parse(typeof(District), values[3]) };
                            
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
