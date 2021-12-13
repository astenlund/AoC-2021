using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day10 : DayBase
{
    private readonly Dictionary<char, uint> CompletionCharScores = new()
    {
        { ')', 1 },
        { ']', 2 },
        { '}', 3 },
        { '>', 4 }
    };

    private readonly Dictionary<char, int> IllegalCharScores = new()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    private readonly Dictionary<char, char> Pairs = new()
    {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' },
        { '<', '>' }
    };

    private string? Input;
    private string[]? Lines;

    internal Day10(string session) : base(session)
    {
    }

    private protected override ushort Day => 10;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var illegalChars = Lines!.Select(FindIllegalChars).Where(ic => ic.Any()).Select(ic => ic.First());
        var result = illegalChars.Select(ic => IllegalCharScores[ic]).Sum();

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var closeChars = Lines!.Where(l => !FindIllegalChars(l).Any()).Select(FindOpenChunks).Select(l => l.Select(c => Pairs[c]));
        var scores = closeChars.Select(l => l.Aggregate(0UL, (acc, c) => 5 * acc + CompletionCharScores[c])).OrderBy(s => s).ToArray();
        var result = scores[(scores.Length - 1) / 2];

        return result.ToString();
    }

    private IEnumerable<char> FindIllegalChars(string line)
    {
        Stack<char> openChunks = new();

        foreach (var c in line)
        {
            if (Pairs.ContainsKey(c))
                openChunks.Push(c);
            if (Pairs.ContainsValue(c))
                if (Pairs[openChunks.Pop()] != c)
                    yield return c;
        }
    }

    private IEnumerable<char> FindOpenChunks(string line)
    {
        var openChunks = new Stack<char>();

        foreach (var c in line)
        {
            if (Pairs.ContainsKey(c))
                openChunks.Push(c);
            if (Pairs.ContainsValue(c))
                openChunks.Pop();
        }

        return openChunks;
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();
        Lines = Regex.Split(Input.Trim(), @"\r?\n");
    }
}
