using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day11 : DayBase
{
    private int[,]? Grid;
    private string[]? Lines;

    internal Day11(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 11;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var result = 100.Times(AdvanceClock).Sum();

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var count = 0;

        while (Grid!.Sum() != 0)
        {
            AdvanceClock();
            count++;
        }

        return count.ToString();
    }

    private int AdvanceClock()
    {
        var width = Grid!.GetLength(0);
        var height = Grid!.GetLength(1);
        var hasFlashed = new bool[width, height];
        var numFlashes = 0;

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            Grid![x, y]++;

        IEnumerable<(int x, int y)> highEnergy;

        while ((highEnergy = GetHighEnergy().ToArray()).Any())
        {
            foreach (var (x, y) in highEnergy)
                if (!hasFlashed[x, y])
                {
                    hasFlashed[x, y] = true;
                    numFlashes++;
                    Grid[x, y] = 0;
                }

            foreach (var (x, y) in highEnergy.SelectMany(GetAdjacent))
                if (!hasFlashed[x, y])
                    Grid[x, y]++;
        }

        return numFlashes;
    }

    private IEnumerable<(int x, int y)> GetAdjacent((int x, int y) p)
    {
        var (x, y) = p;
        for (var y1 = -1; y1 <= 1; y1++)
        for (var x1 = -1; x1 <= 1; x1++)
            if (!OutOfBounds(x + x1, y + y1) && !(x1 == 0 && y1 == 0))
                yield return (x + x1, y + y1);
    }

    private IEnumerable<(int x, int y)> GetHighEnergy()
    {
        for (var y = 0; y < Grid!.GetLength(0); y++)
        for (var x = 0; x < Grid!.GetLength(0); x++)
            if (Grid![x, y] > 9)
                yield return (x, y);
    }

    private bool OutOfBounds(int x, int y) =>
        x < 0 || x > Grid!.GetLength(0) - 1 ||
        y < 0 || y > Grid!.GetLength(1) - 1;

    private protected override async Task Initialize()
    {
        await base.Initialize();

        Lines = Regex.Split(Input!.Trim(), @"\r?\n");
        Grid = new int[Lines[0].Length, Lines.Length];

        for (var y = 0; y < Lines.Length; y++)
        for (var x = 0; x < Lines[0].Length; x++)
            Grid[x, y] = Lines[y][x] - '0';
    }
}
