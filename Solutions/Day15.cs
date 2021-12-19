using System.Text.RegularExpressions;

namespace AoC_2021.Solutions;

public class Day15 : DayBase
{
    private Node? Goal;
    private Node[,]? Grid;
    private Node? Start;

    internal Day15(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 15;

    public override async Task<string> PartOne()
    {
        await Initialize();

        Start = Grid!.Cast<Node>().First();
        Goal = Grid!.Cast<Node>().Last();

        var path = A_Star();
        var result = path.Select(n => n.Risk).Sum() - Start!.Risk;

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        ExpandGrid();

        Start = Grid!.Cast<Node>().First();
        Goal = Grid!.Cast<Node>().Last();

        var path = A_Star();
        var result = path.Select(n => n.Risk).Sum() - Start!.Risk;

        return result.ToString();
    }

    private double Heuristic(Node node) => 1 * (Grid!.GetLength(0) - node.X - 1) + 1 * (Grid!.GetLength(1) - node.Y - 1);

    private IEnumerable<Node> A_Star()
    {
        var fScore = new DefaultDictionary<Node, double>(() => double.MaxValue) { { Start!, Heuristic(Start!) } };
        var gScore = new DefaultDictionary<Node, double>(() => double.MaxValue) { { Start!, 0D } };
        var neighbors = new DefaultDictionary<Node, List<Node>>(() => new List<Node>());
        var openSet = new HashSet<Node> { Start! };
        var cameFrom = new Dictionary<Node, Node>();

        BuildGraph(neighbors);

        while (openSet.Any())
        {
            var current = openSet.MinBy(n => fScore[n]);

            if (current == Goal)
                return ReconstructPath(cameFrom, current!);

            openSet.Remove(current!);

            foreach (var neighbor in neighbors[current!])
            {
                var tentativeGScore = gScore[current!] + neighbor.Risk;
                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current!;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return Enumerable.Empty<Node>();
    }

    private static IEnumerable<Node> ReconstructPath(IDictionary<Node, Node> cameFrom, Node current)
    {
        var totalPath = new LinkedList<Node>(new [] { current });

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.AddFirst(current);
        }

        return totalPath;
    }

    private void BuildGraph(DefaultDictionary<Node, List<Node>> neighbors)
    {
        for (var y = 0; y < Grid!.GetLength(1); y++)
        for (var x = 0; x < Grid!.GetLength(0); x++)
        {
            var node = Grid[x, y];

            if (x > 0)
            {
                var left = Grid[x - 1, y];
                neighbors[left].Add(node);
                neighbors[node].Add(left);
            }

            if (y > 0)
            {
                var top = Grid[x, y - 1];
                neighbors[top].Add(node);
                neighbors[node].Add(top);
            }
        }
    }

    private void ExpandGrid()
    {
        var width = Grid!.GetLength(0);
        var height = Grid!.GetLength(1);
        var newGrid = new Node[width * 5, height * 5];

        for (var dy = 0; dy < 5; dy++)
        for (var dx = 0; dx < 5; dx++)
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var risk = Grid[x, y].Risk + dx + dy;

            while (risk > 9)
                risk -= 9;

            var node = new Node(x + width * dx, y + height * dy, risk);

            newGrid[node.X, node.Y] = node;
        }

        Grid = newGrid;
    }

    private protected override async Task Initialize()
    {
        await base.Initialize();

        var lines = Regex.Split(Input!.Trim(), @"\r?\n").ToArray();

        Grid = new Node[lines[0].Length, lines.Length];

        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[0].Length; x++)
            Grid[x, y] = new Node(x, y, lines[y][x] - '0');
    }

    private sealed record Node(int X, int Y, long Risk);
}
