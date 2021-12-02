namespace AoC_2021;

public abstract class DayBase : IDay
{
    private readonly string Session;

    private protected DayBase(string session) => Session = session;

    public abstract string Name { get; }

    public virtual Task<string> PartOne() => Task.FromResult("Foo!");

    public virtual Task<string> PartTwo() => Task.FromResult("Bar!");

    public override string ToString() => Name;

    private protected async Task<string> GetInput(int day)
    {
        var uri = new Uri($"https://adventofcode.com/2021/day/{day}/input");
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Cookie", $"session={Session}");
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private protected abstract Task Initialize();
}
