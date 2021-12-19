using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day4 : DayBase
{
    private string[]? Lines;

    public Day4(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 4;

    public override async Task<string> PartOne()
    {
        await Initialize();
        return GetWinningBoard().First();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();
        return GetWinningBoard().Last();
    }

    private IEnumerable<string> GetWinningBoard()
    {
        var balls = Lines![0].Split(',').Select(int.Parse);
        var boards = Lines
            .Skip(1)
            .Chunk(5)
            .Select(chunk => chunk.Select(row => Regex.Split(row.Trim(), @"\s+").Select(s => (value: int.Parse(s), marked: false)).ToArray()).ToArray())
            .Select(nums => new[]
            {
                new[] { nums[0][0], nums[0][1], nums[0][2], nums[0][3], nums[0][4] },
                new[] { nums[1][0], nums[1][1], nums[1][2], nums[1][3], nums[1][4] },
                new[] { nums[2][0], nums[2][1], nums[2][2], nums[2][3], nums[2][4] },
                new[] { nums[3][0], nums[3][1], nums[3][2], nums[3][3], nums[3][4] },
                new[] { nums[4][0], nums[4][1], nums[4][2], nums[4][3], nums[4][4] }
            })
            .ToArray();

        var winners = new bool[boards.Length];

        foreach (var ball in balls)
            for (var i = 0; i < boards.Length; i++)
            for (var j = 0; j < 5; j++)
            for (var k = 0; k < 5; k++)
                if (boards[i][j][k].value == ball)
                {
                    boards[i][j][k].marked = true;
                    if (!winners[i] && (CheckRow(boards[i], j) || CheckColumn(boards[i], k)))
                    {
                        winners[i] = true;
                        var sum = boards[i].SelectMany(b => b[..][..]).Where(num1 => !num1.marked).Sum(num2 => num2.value);
                        yield return (sum * ball).ToString();
                    }
                }
    }

    private static bool CheckColumn((int value, bool marked)[][] board, int i) => board.Select(row => row[i]).All(num => num.marked);

    private static bool CheckRow((int value, bool marked)[][] board, int i) => board[i].All(num => num.marked);

    private protected override async Task Initialize()
    {
        await base.Initialize();

        Lines ??= Input!.Trim().Split("\n").Where(l => !string.IsNullOrEmpty(l.Trim())).ToArray();
    }
}
