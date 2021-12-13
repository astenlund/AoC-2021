using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day9 : DayBase
{
    private int[,]? Grid;
    private string? Input;
    private string[]? Lines;

    internal Day9(string session) : base(session)
    {
    }

    private protected override ushort Day => 9;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var lowPoints = FindLowPoints();
        var riskLevels = lowPoints.Select(lp => lp.height + 1);
        var result = riskLevels.Sum();

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var lowPoints = FindLowPoints();
        var visitedPoints = new bool[Grid!.GetLength(0), Grid.GetLength(1)];
        var basins = lowPoints.Select(p => FindBasin(p.x, p.y, visitedPoints));
        var result = basins.Select(b => b.Count()).OrderByDescending(b => b).Take(3).Product();

        return result.ToString();
    }

    // ReSharper disable once TailRecursiveCall
    private IEnumerable<(int x, int y)> FindBasin(int x, int y, bool[,] visitedPoints)
    {
        if (visitedPoints[x, y])
            yield break;

        visitedPoints[x, y] = true;

        if (Grid![x, y] == 9)
            yield break;

        yield return (x, y);

        var w = Grid!.GetLength(0);
        var h = Grid!.GetLength(1);

        if (x > 0)
            foreach (var point in FindBasin(x - 1, y, visitedPoints))
                yield return point;

        if (x < w - 1)
            foreach (var point in FindBasin(x + 1, y, visitedPoints))
                yield return point;

        if (y > 0)
            foreach (var point in FindBasin(x, y - 1, visitedPoints))
                yield return point;

        if (y < h - 1)
            foreach (var point in FindBasin(x, y + 1, visitedPoints))
                yield return point;
    }

    private IEnumerable<(int x, int y, int height)> FindLowPoints()
    {
        var w = Grid!.GetLength(0);
        var h = Grid!.GetLength(1);

        for (var x = 0; x < w; x++)
        for (var y = 0; y < h; y++)
            if (x == 0 || Grid[x, y] < Grid[x - 1, y])
                if (x == w - 1 || Grid[x, y] < Grid[x + 1, y])
                    if (y == 0 || Grid[x, y] < Grid[x, y - 1])
                        if (y == h - 1 || Grid[x, y] < Grid[x, y + 1])
                            yield return (x, y, Grid[x, y]);
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();
        Lines = Regex.Split(Input.Trim(), @"\r?\n");
        Grid = new int[Lines[0].Length, Lines.Length];

        for (var y = 0; y < Lines.Length; y++)
        for (var x = 0; x < Lines[0].Length; x++)
            Grid[x, y] = Lines[y][x] - '0';
    }
}
