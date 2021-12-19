using System.Globalization;

namespace AoC_2021.Solutions;

public class Day7 : DayBase
{
    private int[]? Positions;

    internal Day7(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 7;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var median = Positions!.GetMedian();
        var fuel = Positions!.Aggregate(0d, (acc, pos) => acc + Math.Abs(pos - median));

        return fuel.ToString(CultureInfo.CurrentCulture);
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var fuel = Enumerable.Range(0, Positions!.Max()).Aggregate(int.MaxValue, (acc, idx) => Math.Min(acc, CalculateTotalFuel(idx)));

        return fuel.ToString();
    }

    private int CalculateTotalFuel(int alignment) =>
        Positions!
            .Select(position => Math.Abs(position - alignment))
            .Select(distance => distance * (distance + 1) / 2)
            .Sum();

    private protected override async Task Initialize()
    {
        await base.Initialize();

        Positions = Input!.Split(',').Select(int.Parse).ToArray();
    }
}
