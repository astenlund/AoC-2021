using System.Drawing;

namespace AoC_2021;

public class Day2 : DayBase
{
    private string? Input;
    private List<(string, int)>? Lines;

    public Day2(string session) : base(session)
    {
    }

    private protected override ushort Day => 2;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var pos = new Size(0, 0);

        pos = Lines!.Aggregate(pos, (current, line) =>
            current + line switch
            {
                ("forward", var value)  => new Size(value, 0),
                ("up", var value) => new Size(0, -value),
                ("down", var value) => new Size(0, value),
                _ => new Size(0, 0)
            });

        return (pos.Width * pos.Height).ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var aim = 0;
        var pos = new Size(0, 0);

        foreach (var line in Lines!)
        {
            aim += line switch
            {
                ("up", var value) => -value,
                ("down", var value) => value,
                _ => 0
            };

            pos += line switch
            {
                ("forward", var value)  => new Size(value, aim * value),
                _ => new Size(0, 0)
            };
        }

        return (pos.Width * pos.Height).ToString();
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();
        Lines ??= Input.Trim().Split("\n").Select(l =>
        {
            var split = l.Split(" ");
            return (split[0], int.Parse(split[1]));
        }).ToList();
    }
}
