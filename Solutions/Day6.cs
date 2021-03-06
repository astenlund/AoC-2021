using System.Numerics;

namespace AoC_2021.Solutions;

public class Day6 : DayBase
{
    private BigInteger[]? FishBuckets;

    public Day6(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 6;

    public override async Task<string> PartOne()
    {
        await Initialize();

        80.Times(AdvanceClock);

        return FishBuckets!.Sum().ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        256.Times(AdvanceClock);

        return FishBuckets!.Sum().ToString();
    }

    private void AdvanceClock()
    {
        var expired = FishBuckets![0];
        FishBuckets[0] = 0;

        for (var i = 1; i < FishBuckets.Length; i++)
        {
            FishBuckets[i - 1] += FishBuckets[i];
            FishBuckets[i] = 0;
        }

        FishBuckets[6] += expired;
        FishBuckets[8] += expired;
    }

    private protected override async Task Initialize()
    {
        await base.Initialize();

        FishBuckets = new BigInteger[9];

        foreach (var timer in Input!.Split(',').Select(uint.Parse))
            FishBuckets[timer]++;
    }
}
