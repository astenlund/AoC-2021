using System.Text.RegularExpressions;

namespace AoC_2021;

public class Day5 : DayBase
{
    private string? Input;
    private string[]? Lines;
    private (int x1, int y1, int x2, int y2)[]? Vectors;

    public Day5(string session) : base(session)
    {
    }

    public override string Name => "Day 5";

    public override async Task<string> PartOne()
    {
        await Initialize();

        return GetNumberOfPointsWhereAtLeastTwoLinesOverlap(false);
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        return GetNumberOfPointsWhereAtLeastTwoLinesOverlap(true);
    }

    private string GetNumberOfPointsWhereAtLeastTwoLinesOverlap(bool includeDiagonals)
    {
        var (xMax, yMax) = FindDimensions();
        var points = new int[xMax, yMax];

        foreach (var (x1, y1, x2, y2) in Vectors!)
            if (IsHorizontalOrVertical(x1, y1, x2, y2) || IsDiagonal(x1, y1, x2, y2) && includeDiagonals)
            {
                var dx = x1 < x2 ? 1 : x1 > x2 ? -1 : 0;
                var dy = y1 < y2 ? 1 : y1 > y2 ? -1 : 0;
                var (x, y) = (x1 - dx, y1 - dy);

                while (x != x2 || y != y2)
                {
                    if (x != x2) x += dx;
                    if (y != y2) y += dy;
                    points[x, y]++;
                }
            }

        return points.Cast<int>().Count(p => p >= 2).ToString();
    }

    private static bool IsHorizontalOrVertical(int x1, int y1, int x2, int y2) =>
        x1 == x2 || y1 == y2;

    private static bool IsDiagonal(int x1, int y1, int x2, int y2) =>
        x1 != x2 && y1 != y2 &&
        Math.Abs(x2 - x1) == Math.Abs(y2 - y1);

    private (int xMax, int yMax) FindDimensions()
    {
        var (xMax, yMax) = (0, 0);

        foreach (var (x1, y1, x2, y2) in Vectors!)
        {
            xMax = Math.Max(xMax, x1);
            xMax = Math.Max(xMax, x2);
            yMax = Math.Max(yMax, y1);
            yMax = Math.Max(yMax, y2);
        }

        return (xMax + 1, yMax + 1);
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput(5);
        Lines ??= Input.Trim().Split("\n").Where(l => !string.IsNullOrEmpty(l.Trim())).ToArray();
        Vectors ??= Lines.Select(l =>
        {
            var match = Regex.Match(l, @"(\d+),(\d+) -> (\d+),(\d+)");
            var x1 = int.Parse(match.Groups[1].Value);
            var y1 = int.Parse(match.Groups[2].Value);
            var x2 = int.Parse(match.Groups[3].Value);
            var y2 = int.Parse(match.Groups[4].Value);
            return (x1, y1, x2, y2);
        }).ToArray();
    }
}
