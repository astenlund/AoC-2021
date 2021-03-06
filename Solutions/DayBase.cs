namespace AoC_2021.Solutions;

public abstract class DayBase : IDay
{
    private readonly string Session;
    private protected string? Input;

    private protected DayBase(string session, string? input)
    {
        Session = session;
        Input = input;
    }

    private protected abstract ushort Day { get; }

    public abstract Task<string> PartOne();

    public abstract Task<string> PartTwo();

    public override string ToString() => $"Day {Day}";

    private async Task<string> GetInput()
    {
        var uri = new Uri($"https://adventofcode.com/2021/day/{Day}/input");
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Cookie", $"session={Session}");
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private protected virtual async Task Initialize()
    {
        Input ??= await GetInput();
    }
}
