using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day14 : DayBase
{
    private string? Input;
    private Dictionary<(char left, char right), char>? Rules;
    private string? Template;

    internal Day14(string session) : base(session)
    {
    }

    private protected override ushort Day => 14;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var elements = Polymerize(10);
        var ordered = elements.OrderByDescending(e => e.Value).ToArray();
        var result = ordered[0].Value - ordered[^1].Value;

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var elements = Polymerize(40);
        var ordered = elements.OrderByDescending(e => e.Value).ToArray();
        var result = ordered[0].Value - ordered[^1].Value;

        return result.ToString();
    }

    private IDictionary<char, ulong> Polymerize(int iterations)
    {
        var pairs = new DefaultDictionary<(char left, char right), ulong>(() => 0UL);
        var elements = new DefaultDictionary<char, ulong>(() => 0UL) { { Template![0], 1 } };

        for (var i = 1; i < Template!.Length; i++)
        {
            var left = Template[i - 1];
            var right = Template[i];

            pairs[(left, right)]++;
            elements[right]++;
        }

        for (var i = 0; i < iterations; i++)
        {
            var newPairs = new DefaultDictionary<(char left, char right), ulong>(() => 0UL);

            foreach (var rule in Rules!.Where(rule => pairs.ContainsKey(rule.Key)))
            {
                var (left, right) = rule.Key;
                var center = rule.Value;

                if (pairs.ContainsKey((left, right)))
                {
                    var count = pairs[(left, right)];
                    pairs.Remove((left, right));
                    newPairs[(left, center)] += count;
                    newPairs[(center, right)] += count;
                    elements[center] += count;
                }
            }

            foreach (var ((left, right), count) in newPairs)
                pairs[(left, right)] += count;
        }

        return elements;
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();

        var lines = Regex.Split(Input.Trim(), @"\r?\n").ToArray();

        Template = lines.First();
        Rules = lines.Skip(2).Select(RuleSelector).ToDictionary();

        KeyValuePair<(char left, char right), char> RuleSelector(string line)
        {
            var match = Regex.Match(line, @"([A-Z]{2}) -> ([A-Z]+){1}");
            var key = match.Groups[1].Value;
            var value = match.Groups[2].Value;

            return KeyValuePair.Create((key[0], key[1]), value[0]);
        }
    }
}
