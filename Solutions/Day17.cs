using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day17 : DayBase
{
    private ((int min, int max) x, (int min, int max) y) Target;

    internal Day17(string session, string? input) : base(session, input)
    {
    }

    private protected override ushort Day => 17;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var result = RapidFire().Select(s => s.yMax).Max();

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var result = RapidFire().Count();

        return result.ToString();
    }

    private IEnumerable<(int vx, int vy, int yMax)> RapidFire()
    {
        for (var vy = Target.y.min; vy <= -Target.y.min; vy++)
        for (var vx = 0; vx <= Target.x.max; vx++)
        {
            var (hit, yMax) = Fire(vx, vy);

            if (hit)
                yield return (vx, vy, yMax);
        }
    }

    private (bool hit, int yMax) Fire(int vx, int vy)
    {
        var (x, y, yMax, hit) = (0, 0, 0, false);

        while (x <= Target.x.max && y >= Target.y.min)
        {
            x += vx;
            y += vy;

            yMax = Math.Max(y, yMax);

            if (IsWithinTargetArea(x, y))
            {
                hit = true;
                break;
            }

            vx += vx switch
            {
                < 0 => +1,
                > 0 => -1,
                0 =>  0
            };
            --vy;
        }

        return (hit, yMax);
    }

    private bool IsWithinTargetArea(int x, int y) =>
        Target.x.min <= x && x <= Target.x.max &&
        Target.y.min <= y && y <= Target.y.max;

    private protected override async Task Initialize()
    {
        await base.Initialize();

        var match = Regex.Match(Input!, @"target area: x=(\-?\d+)\.\.(\-?\d+), y=(\-?\d+)\.\.(\-?\d+)");

        var x1 = int.Parse(match.Groups[1].Value);
        var x2 = int.Parse(match.Groups[2].Value);
        var y1 = int.Parse(match.Groups[3].Value);
        var y2 = int.Parse(match.Groups[4].Value);

        Target = (x: (min: Math.Min(x1, x2), max: Math.Max(x1, x2)), y: (min: Math.Min(y1, y2), max: Math.Max(y1, y2)));
    }
}
