using System.Text;
using System.Text.RegularExpressions;
using static System.Environment;

namespace AoC_2021.Solutions;

public class Day13 : DayBase
{
    private const char Left = 'x';
    private const char Up = 'y';

    private HashSet<(int x, int y)>? Dots;
    private (char dir, int pos)[]? FoldInstructions;
    private string? Input;

    internal Day13(string session) : base(session)
    {
    }

    private protected override ushort Day => 13;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var (dir, pos) = FoldInstructions![0];

        Dots = Fold(dir, pos, Dots!).ToHashSet();

        return Dots!.Count.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        foreach (var (dir, pos) in FoldInstructions!)
            Dots = Fold(dir, pos, Dots!).ToHashSet();

        return FormatDots();
    }

    private static IEnumerable<(int x, int y)> Fold(char dir, int pos, IEnumerable<(int x, int y)> dots)
    {
        foreach (var (x, y) in dots)
            yield return dir switch
            {
                Left => (pos - Math.Abs(pos - x), y),
                Up => (x, pos - Math.Abs(pos - y)),
                _ => throw new InvalidOperationException()
            };
    }

    private string FormatDots()
    {
        var xMax = Dots!.Select(d => d.x).Max();
        var yMax = Dots!.Select(d => d.y).Max();
        var grid = new bool[xMax + 1, yMax + 1];
        var s = new StringBuilder();

        foreach (var (x, y) in Dots!)
            grid[x, y] = true;

        for (var y = 0; y <= yMax; y++)
        for (var x = 0; x <= xMax; x++)
        {
            s.Append(grid[x, y] ? '#' : '.');
            if (x == xMax && y != yMax)
                s.Append(NewLine);
        }

        return s.ToString();
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();

        var lines = Regex.Split(Input.Trim(), @"\r?\n").ToArray();

        Dots = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(DotSelector).ToHashSet();
        FoldInstructions = lines.Where(l => l.StartsWith("fold")).Select(FoldSelector).ToArray();

        (int x, int y) DotSelector(string line)
        {
            var ints = line.Split(',');
            var x = int.Parse(ints[0]);
            var y = int.Parse(ints[1]);

            return (x, y);
        }

        (char dir, int pos) FoldSelector(string line)
        {
            var match = Regex.Match(line, @"fold along (x|y)=(\d+)");
            var dir = match.Groups[1].Value.ToCharArray()[0];
            var pos = int.Parse(match.Groups[2].Value);

            return (dir, pos);
        }
    }
}
