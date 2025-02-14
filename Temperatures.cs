namespace ConsoleApp1;

public class Temperatures
{
    private static readonly List<ColumnDef> TemperatureDatDefinition =
    [
        new(TemperatureCol.Dd, 1, 3),
        new(TemperatureCol.Max, 5, 3, ['*']),
        new(TemperatureCol.Min, 11, 5, ['*']),
        new(TemperatureCol.Avg, 17, 5),
        new(TemperatureCol.Dxc, 24, 4),
    ];

    private static readonly string FileName = "temperature.dat";

    public static void Run()
    {
        DatParser pTemp = new DatParser(TemperatureDatDefinition, 4, skipLinesEnd: 1);
        String filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

        List<Dictionary<Enum, string?>> result;
        using (StreamReader temperatureStream = File.OpenText(filePath))
        {
            // Get data from DAT file
            result = pTemp.ParseFile(temperatureStream);
        }

        // Process parsed DAT into usable format for calculations
        Dictionary<int, (int, int)> dayTempSpread = [];
        foreach (var dictionary in result)
        {
            // Check if all values are of correct type, ignore otherwise 
            if (int.TryParse(dictionary[TemperatureCol.Dd], out var day)
                && int.TryParse(dictionary[TemperatureCol.Max], out var max)
                && int.TryParse(dictionary[TemperatureCol.Min], out var min))
            {
                dayTempSpread[day] = (min, max);
            }
        }

        // Calculate minimum spread between Min and Max temperature
        var minSpread = dayTempSpread.MinBy(x => x.Value.Item2 - x.Value.Item1);

        // Report result
        Console.WriteLine("Temperature Smallest Spread");
        Console.WriteLine("Day: {0} | Min: {1}, Max: {2}, Spread: {3}",
            minSpread.Key, minSpread.Value.Item1, minSpread.Value.Item2, minSpread.Value.Item2 - minSpread.Value.Item1);
        Console.WriteLine();
    }
}