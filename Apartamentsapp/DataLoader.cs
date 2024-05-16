namespace Apartamentsapp
{
    public class DataLoader
    {
        public static void ReadDataFromCSV(string filePath, Dictionary<string, Flats> flatsDictionary)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines.Skip(1))
            {
                string[] values = line.Split(',');
                string flatName = values[0];
                int price = int.Parse(values[1]);
                int area = int.Parse(values[2]);
                string district = values[3];
                flatsDictionary[flatName] = new Flats {FlatName = flatName, ApartmentPrice = price, Area = area, District = district };
            }
        }
    }
}
