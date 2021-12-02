namespace AoC_2021;

public interface IDay
{
    string Name { get; }
    Task<string> PartOne();
    Task<string> PartTwo();
}
