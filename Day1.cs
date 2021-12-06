namespace AoC_2021;

public class Day1 : DayBase
{
    private string? Input;
    private List<int>? Lines;

    public Day1(string session) : base(session)
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
        Input ??= await GetInput();
        Lines ??= Input.Trim().Split("\n").Select(int.Parse).ToList();
    }
}
