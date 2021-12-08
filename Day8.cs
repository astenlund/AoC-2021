using System.Text.RegularExpressions;

namespace AoC_2021;

public class Day8 : DayBase
{
    private const string AllSegments = "abcdefg";

    private readonly Dictionary<string, int> DigitsBySegment = new()
    {
        { "abcefg", 0 }, { "cf", 1 }, { "acdeg", 2 }, { "acdfg", 3 }, { "bcdf", 4 },
        { "abdfg", 5 }, { "abdefg", 6 }, { "acf", 7 }, { "abcdefg", 8 }, { "abcdfg", 9 }
    };

    private readonly Dictionary<int, string> SegmentsByDigit = new()
    {
        { 0, "abcefg" }, { 1, "cf" }, { 2, "acdeg" }, { 3, "acdfg" }, { 4, "bcdf" },
        { 5, "abdfg" }, { 6, "abdefg" }, { 7, "acf" }, { 8, "abcdefg" }, { 9, "abcdfg" }
    };

    private (string[] patterns, string[] output)[]? Entries;

    private string? Input;
    private string[]? Lines;

    internal Day8(string session) : base(session)
    {
    }

    private protected override ushort Day => 8;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var targetDigits = new[] { 1, 4, 7, 8 };
        var result = Decipher().SelectMany(digits => digits.Select(digit => digit)).Count(digit => targetDigits.Contains(digit));

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var result = Decipher().Select(digits => digits.Concat()).Sum();

        return result.ToString();
    }

    private IEnumerable<IEnumerable<int>> Decipher()
    {
        foreach (var (patterns, outputDigits) in Entries!)
        {
            var alphabet = AllSegments.ToDictionary(c => c, _ => AllSegments);

            // Find digit: 1
            var p1 = FindPatternByNumSegments(patterns, 2);
            foreach (var letter in p1)
                alphabet[letter] = alphabet[letter].Intersect(SegmentsByDigit[1]).Concat();

            // Find digit: 4
            var p4 = FindPatternByNumSegments(patterns, 4);
            foreach (var letter in p4)
                alphabet[letter] = alphabet[letter].Intersect(SegmentsByDigit[4]).Concat();

            // Find digit: 7
            var p7 = FindPatternByNumSegments(patterns, 3);
            foreach (var letter in p7)
                alphabet[letter] = alphabet[letter].Intersect(SegmentsByDigit[7]).Concat();

            // Find segment: a
            var a = p7.Except(p1).Single();
            foreach (var letter in AllSegments)
                alphabet[letter] = alphabet[letter].Except("a").Concat();
            alphabet[a] = "a";

            // Find segments: b; d
            var bd = p4.Except(p1).Concat();
            foreach (var letter in AllSegments.Except(bd))
                alphabet[letter] = alphabet[letter].Except("bd").Concat();
            foreach (var letter in bd)
                alphabet[letter] = alphabet[letter].Intersect("bd").Concat();

            // Find segments: c; d; e
            var cde = patterns.Where(p => p.Length == 6).Aggregate("", (acc, pattern) => acc + AllSegments.Except(pattern).Single());
            foreach (var letter in AllSegments.Except(cde))
                alphabet[letter] = alphabet[letter].Except("cde").Concat();
            foreach (var letter in cde)
                alphabet[letter] = alphabet[letter].Intersect("cde").Concat();

            // Remove extra segments when unique segment has been found
            foreach (var (letter, segments) in alphabet.Where(pair => pair.Value.Length > 1))
            foreach (var segment in segments.Where(segment => alphabet.Values.SelectMany(s => s).Count(c => c == segment) == 1))
                alphabet[letter] = segment.ToString();

            yield return outputDigits.Select(d => d.Select(l => alphabet[l][0]).OrderBy(l => l).Concat()).Select(s => DigitsBySegment[s]);
        }
    }

    private static string FindPatternByNumSegments(IEnumerable<string> patterns, int numSegments) => patterns.First(pattern => pattern.Length == numSegments);

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();
        Lines = Regex.Split(Input.Trim(), @"\r?\n");
        Entries = Lines.Select(line =>
        {
            var strs = Regex.Split(line.Trim(), @"\s*\|\s*");
            var patterns = Regex.Split(strs[0], @"\s+");
            var output = Regex.Split(strs[1], @"\s+");
            return (patterns, output);
        }).ToArray();
    }
}
