using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day12 : DayBase
{
    private Dictionary<string, List<string>>? Connections;
    private string? Input;

    internal Day12(string session) : base(session)
    {
    }

    private protected override ushort Day => 12;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var visits = Connections!.Keys.ToDictionary(c => c, _ => 0);
        var result = Traverse("start", "", visits, (v, c) => v[c] == 0 || c.IsUpper()).Count();

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        bool Predicate(IDictionary<string, int> v, string c) =>
            v[c] == 0 ||
            c.IsUpper() ||
            c != "start" && c != "end" && v.Where(kvp => kvp.Key.IsLower()).All(kvp => kvp.Value < 2);

        var visits = Connections!.Keys.ToDictionary(c => c, _ => 0);
        var result = Traverse("start", "", visits, Predicate).Count();

        return result.ToString();
    }

    private IEnumerable<string> Traverse(string cave, string path, IDictionary<string, int> visits, Func<IDictionary<string, int>, string, bool> predicate)
    {
        path = string.IsNullOrEmpty(path) ? cave : $"{path},{cave}";
        visits[cave]++;

        if (cave == "end")
            yield return path;

        var connectedCaves = Connections![cave].Where(c => predicate(visits, c));

        foreach (var connectedCave in connectedCaves)
        foreach (var completedPath in Traverse(connectedCave, path, new Dictionary<string, int>(visits), predicate))
            yield return completedPath;
    }

    private protected override async Task Initialize()
    {
        Input ??= await GetInput();

        var lines = Regex.Split(Input.Trim(), @"\r?\n");
        var edges = lines.Select(l => l.Split('-')).ToArray();
        var caves = edges.SelectMany(e => e).Distinct().ToArray();

        Connections = caves.ToDictionary(c => c, _ => new List<string>());

        foreach (var edge in edges)
        {
            Connections[edge[0]].Add(edge[1]);
            Connections[edge[1]].Add(edge[0]);
        }
    }
}
