using System.Collections;

namespace AoC_2021.Solutions;

public class Day3 : DayBase
{
    private string? Input;
    private string[]? Lines;

    public Day3(string session) : base(session)
    {
    }

    private protected override ushort Day => 3;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var numOnes = GetNumberOfOnesPerPos(Lines!);
        var threshold = Lines!.Length / 2d;
        var gammaBits = new BitArray(numOnes.Length);
        var epsilonBits = new BitArray(numOnes.Length);

        for (var i = 0; i < numOnes.Length; i++)
        {
            var value = numOnes[i] > threshold;
            gammaBits[numOnes.Length - 1 - i] = value;
            epsilonBits[numOnes.Length - 1 - i] = !value;
        }

        var gamma = gammaBits.ToUInt();
        var epsilon = epsilonBits.ToUInt();

        return (gamma * epsilon).ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var oxyRatingCandidates = Lines!;
        var co2RatingCandidates = Lines!;

        for (var i = 0; i < Lines![0].Length && oxyRatingCandidates.Length > 1; i++)
        {
            var numOnes = GetNumberOfOnesAtPosition(oxyRatingCandidates, i);
            var threshold = oxyRatingCandidates.Length / 2d;
            var mostCommonBit = numOnes >= threshold ? '1' : '0';
            oxyRatingCandidates = oxyRatingCandidates.Where(o => o[i] == mostCommonBit).ToArray();
        }

        for (var i = 0; i < Lines![0].Length && co2RatingCandidates.Length > 1; i++)
        {
            var numOnes = GetNumberOfOnesAtPosition(co2RatingCandidates, i);
            var threshold = co2RatingCandidates.Length / 2d;
            var leastCommonBit = numOnes >= threshold ? '0' : '1';
            co2RatingCandidates = co2RatingCandidates.Where(o => o[i] == leastCommonBit).ToArray();
        }

        var oxyRating = Convert.ToInt32(oxyRatingCandidates.Single(), 2);
        var co2Rating = Convert.ToInt32(co2RatingCandidates.Single(), 2);

        return (oxyRating * co2Rating).ToString();
    }

    private static uint[] GetNumberOfOnesPerPos(IReadOnlyList<string> lines)
    {
        var ones = new uint[lines[0].Length];

        foreach (var line in lines)
            for (var i = 0; i < line.Length; i++)
                if (line[i] == '1')
                    ones[i]++;

        return ones;
    }

    private static uint GetNumberOfOnesAtPosition(IEnumerable<string> lines, int i)
    {
        var ones = 0u;

        foreach (var line in lines)
            if (line[i] == '1')
                ones++;

        return ones;
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();
        Lines = Input.Trim().Split("\n");
    }
}
