namespace ConsoleApp1;

public class ColumnDef(Enum name, int start, int length, char[]? replacements = null)

{
    public int Start { get; } = start;

    public int Length { get; } = length;

    public Enum Name { get; } = name;

    public char[] Replacements => replacements ?? [];
}