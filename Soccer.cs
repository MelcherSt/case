namespace ConsoleApp1;

public class Soccer
{
    private static readonly List<ColumnDef> SoccerDatDefinition =
    [
        new(SoccerCol.City, 7, 16),
        new(SoccerCol.P1, 23, 2),
        new(SoccerCol.W, 29, 2),
        new(SoccerCol.L, 33, 2),
        new(SoccerCol.D, 37, 2),
        new(SoccerCol.F, 43, 2),
        new(SoccerCol.A, 50, 2),
        new(SoccerCol.P2, 6, 2)
    ];

    private static readonly string FileName = "socker.dat";

    public static void Run()
    {
        DatParser pSoccer = new DatParser(SoccerDatDefinition, 1, globalReplacements: ['-']);
        String filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

        List<Dictionary<Enum, string?>> result;
        using (StreamReader soccerStream = File.OpenText(filePath))
        {
            result = pSoccer.ParseFile(soccerStream);
        }

        // Process parsed DAT into usable format for calculations
        Dictionary<String, (int, int)> soccerGoals = [];
        foreach (var dictionary in result)
        {
            var city = dictionary[SoccerCol.City];
            if (city == null)
            {
                continue;
            }

            // Check if all values are of correct type, ignore otherwise 
            if (int.TryParse(dictionary[SoccerCol.F], out var gFor)
                && int.TryParse(dictionary[SoccerCol.A], out var gAgainst))
            {
                soccerGoals[city] = (gFor, gAgainst);
            }
        }

        var minDif = soccerGoals.MinBy(x => Math.Abs(x.Value.Item2 - x.Value.Item1));

        // Report result
        Console.WriteLine("Soccer Smallest Differene For and Against Goals");
        Console.WriteLine("Club: {0} | For: {1}, Against: {2}, Spread: {3}",
            minDif.Key, minDif.Value.Item1, minDif.Value.Item2, minDif.Value.Item2 - minDif.Value.Item1);
        Console.WriteLine();
    }
}