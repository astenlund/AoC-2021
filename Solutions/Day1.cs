namespace AoC_2021.Solutions;

public class Day1 : DayBase
{
    private List<int>? Lines;

    public Day1(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 1;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var count = 0;

        for (var i = 0; i < Lines!.Count - 1; i++)
            if (Lines[i + 1] > Lines[i])
                count++;

        return count.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var (prevSum, count) = (int.MaxValue, 0);

        for (var i = 0; i < Lines!.Count - 2; i++)
        {
            var sum = Lines[i] + Lines[i + 1] + Lines[i + 2];
            if (sum > prevSum) count++;
            prevSum = sum;
        }

        return count.ToString();
    }

    private protected override async Task Initialize()
    {
        await base.Initialize();

        Lines ??= Input!.Trim().Split("\n").Select(int.Parse).ToList();
    }
}
