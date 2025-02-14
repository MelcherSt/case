namespace ConsoleApp1;

public class DatParser
{
    private readonly List<ColumnDef> _columns;
    private readonly int _skipLines;
    private readonly int _skipLinesEnd;
    private readonly char[]? _globalReplacements;

    public DatParser(List<ColumnDef> datDefinition, int skipLines, char[]? globalReplacements = null,
        int skipLinesEnd = 0)
    {
        _columns = datDefinition;
        _skipLines = skipLines;
        _skipLinesEnd = skipLinesEnd;
        _globalReplacements = globalReplacements;
    }

    public List<Dictionary<Enum, string?>> ParseFile(StreamReader stream)
    {
        List<Dictionary<Enum, string?>> result = [];

        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        try
        {
            // Skip the required amount of lines
            for (int i = 0; i < _skipLines; i++)
            {
                stream.ReadLine();
            }

            int lineNumber = 0;
            String? line;
            while ((line = stream.ReadLine()) != null)
            {
                // Extract every defined column 
                Dictionary<Enum, string?> lineResult = new Dictionary<Enum, string?>();
                foreach (ColumnDef def in _columns)
                {
                    String? value = null;
                    try
                    {
                        value = line.Substring(def.Start, def.Length).Trim();
                        foreach (var repl in def.Replacements.Concat(_globalReplacements ?? []))
                        {
                            value = value.Replace(repl.ToString(), "");
                        }

                        if (String.IsNullOrEmpty(value))
                        {
                            value = null;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        // File does not adhere to definition. Continue
                        Console.WriteLine("Line {0}, Column {1}: incorrect format", lineNumber, def.Name);
                    }

                    lineResult[def.Name] = value;
                }

                if (lineResult.Values.Any(x => x != null))
                {
                    result.Add(lineResult);
                }

                lineNumber++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        if (_skipLinesEnd > 0)
        {
            result = result.Slice(0, result.Count - _skipLinesEnd);
        }

        return result;
    }
}