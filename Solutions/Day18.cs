using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day18 : DayBase
{
    private string[]? Lines;

    internal Day18(string session, string? input) : base(session, input)
    {
    }

    private protected override ushort Day => 18;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var numbers = Lines!.Aggregate("", (acc, line) => Reduce(Add(acc, line)));

        return CalculateMagnitude(numbers).ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        return Lines!
            .Permute()
            .Select(t => Add(t.first, t.second))
            .Select(Reduce)
            .Select(CalculateMagnitude)
            .Max()
            .ToString();
    }

    private static string Add(string n1, string n2)
    {
        if (n1 == "")
            return n2;

        if (n2 == "")
            return n1;

        return Reduce($"[{n1},{n2}]");
    }

    private static int CalculateMagnitude(string numbers)
    {
        Match match;

        while ((match = Regex.Match(numbers, @"\[(\d+),(\d+)\]")).Success)
        {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            var m = 3 * x + 2 * y;

            numbers = numbers.Replace(match.Value, m.ToString());
        }

        return int.Parse(numbers);
    }

    private static string Reduce(string numbers)
    {
        do
        {
            do
            {
            } while (TryExplode(ref numbers));
        } while (TrySplit(ref numbers));

        return numbers;
    }

    private static bool TryExplode(ref string numbers)
    {
        var level = 0;
        var left = 0;

        for (var i = 0; i < numbers.Length; i++)
        {
            level += numbers[i] switch
            {
                '[' =>  1,
                ']' => -1,
                _   =>  0
            };

            if (level == 5)
            {
                left = i;
                break;
            }
        }

        if (left == 0)
            return false;

        var right = numbers.IndexOf(']', left) + 1;
        var pair = numbers[left..right];
        var match = Regex.Match(pair, @"\[(\d+),(\d+)\]");

        var x = int.Parse(match.Groups[1].Value);
        var y = int.Parse(match.Groups[2].Value);

        numbers = numbers[..left] + 0 + numbers[right..];

        var rightLeft = 0;
        var rightRight = 0;

        for (var i = left + 1; i < numbers.Length && rightRight == 0; i++)
        {
            if (rightLeft == 0 && char.IsDigit(numbers[i]))
                rightLeft = i;
            if (rightLeft != 0 && !char.IsDigit(numbers[i]))
                rightRight = i;
        }

        if (rightRight != 0)
        {
            var rightNumber = y + int.Parse(numbers[rightLeft..rightRight]);
            numbers = numbers[..rightLeft] + rightNumber + numbers[rightRight..];
        }

        var leftLeft = 0;
        var leftRight = 0;

        for (var i = left - 1; i > 0 && leftLeft == 0; i--)
        {
            if (leftRight == 0 && char.IsDigit(numbers[i]))
                leftRight = i + 1;
            if (leftRight != 0 && !char.IsDigit(numbers[i]))
                leftLeft = i + 1;
        }

        if (leftLeft != 0)
        {
            var leftNumber = x + int.Parse(numbers[leftLeft..leftRight]);
            numbers = numbers[..leftLeft] + leftNumber + numbers[leftRight..];
        }

        return true;
    }

    private static bool TrySplit(ref string numbers)
    {
        var left = 0;
        var right = 0;

        for (var i = 0; i < numbers.Length && right == 0; i++)
        {
            if (left == 0 && char.IsDigit(numbers[i]))
                left = i;
            if (left != 0 && !char.IsDigit(numbers[i]))
                right = i;
            if (right != 0 && right - left < 2)
            {
                left = 0;
                right = 0;
            }
        }

        if (right == 0)
            return false;

        var n = int.Parse(numbers[left..right]);
        var x = Math.Floor(n / 2D);
        var y = Math.Ceiling(n / 2D);

        numbers = numbers[..left] + $"[{x},{y}]" + numbers[right..];

        return true;
    }

    private protected override async Task Initialize()
    {
        await base.Initialize();

        Lines = Regex.Split(Input!.Trim(), @"\r?\n");
    }
}
