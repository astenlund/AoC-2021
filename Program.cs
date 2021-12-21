using System.Diagnostics;
using System.Text.RegularExpressions;
using AoC_2021;
using AoC_2021.Solutions;
using Microsoft.Extensions.Configuration;
using Terminal.Gui;
using static System.Environment;
using static Terminal.Gui.View;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var sessionId = config["SessionId"];
var selectedDay = default(IDay);
var digitKeyHandler = new DigitKeyHandler(TimeSpan.FromMilliseconds(300));
var input = args.Any() ? args[0] : null;

Application.Init();

List<IDay> days = new()
{
    new Day1(sessionId, input),
    new Day2(sessionId, input),
    new Day3(sessionId, input),
    new Day4(sessionId, input),
    new Day5(sessionId, input),
    new Day6(sessionId, input),
    new Day7(sessionId, input),
    new Day8(sessionId, input),
    new Day9(sessionId, input),
    new Day10(sessionId, input),
    new Day11(sessionId, input),
    new Day12(sessionId, input),
    new Day13(sessionId, input),
    new Day14(sessionId, input),
    new Day15(sessionId, input),
    new Day16(sessionId, input),
    new Day17(sessionId, input)
};

List<string> parts = new()
{
    "Back",
    "Part 1",
    "Part 2"
};

var title = new Label("Advent of Code 2021")
{
    X = Pos.Center(),
    Y = 0,
    Height = 1
};

var leftPane = new FrameView("Puzzles")
{
    X = 0,
    Y = 1,
    Width = 50,
    Height = Dim.Fill(1),
    CanFocus = false
};

var dayList = new ListView(days)
{
    X = 0,
    Y = 0,
    Width = Dim.Fill(),
    Height = Dim.Fill(),
    AllowsMarking = false,
    CanFocus = true
};

var partList = new ListView(parts)
{
    X = 0,
    Y = 0,
    Width = Dim.Fill(),
    Height = Dim.Fill(),
    AllowsMarking = false,
    CanFocus = true
};

leftPane.Add(dayList);

var rightPane = new FrameView("Output")
{
    X = 50,
    Y = 1,
    Width = Dim.Fill(),
    Height = Dim.Fill(1),
    CanFocus = false
};

var outputView = new TextView
{
    X = 0,
    Y = 0,
    Width = Dim.Fill(),
    Height = Dim.Fill(),
    CanFocus = false
};

dayList.OpenSelectedItem += eventArgs =>
{
    selectedDay = (IDay)eventArgs.Value;
    leftPane.RemoveAll();
    leftPane.Add(partList);
    leftPane.Title = "Puzzle: " + eventArgs.Value;
    partList.SelectedItem = 0;
    partList.SetNeedsDisplay();
    partList.SetFocus();
};

partList.OpenSelectedItem += async eventArgs =>
{
    switch (eventArgs.Item)
    {
        case 0:
            leftPane.RemoveAll();
            leftPane.Add(dayList);
            leftPane.Title = "Puzzles";
            dayList.SetFocus();
            break;
        case 1:
            rightPane.Title = $"Output: {selectedDay}, Part 1";
            var (result1, time1) = await Measure(async () => await selectedDay!.PartOne());
            outputView.Text = FormatResult(result1);
            rightPane.Title += $" ({FormatTime(time1)})";
            break;
        case 2:
            rightPane.Title = $"Output: {selectedDay}, Part 2";
            var (result2, time2) = await Measure(async () => await selectedDay!.PartTwo());
            outputView.Text = FormatResult(result2);
            rightPane.Title += $" ({FormatTime(time2)})";
            break;
    }
};

rightPane.Add(outputView);

var statusBar = new StatusBar
{
    Visible = true
};

statusBar.Items = new []
{
    new StatusItem(Key.C | Key.CtrlMask, "~CTRL-C~ Quit", () => Application.RequestStop())
};

Application.Top.KeyDown += KeyDownHandler;

Application.Top.Add(title);
Application.Top.Add(leftPane);
Application.Top.Add(rightPane);
Application.Top.Add(statusBar);

Application.Run();

string FormatResult(string output) => NewLine + " " + string.Join(NewLine + " ", Regex.Split(output, @"\r?\n"));

string FormatTime(TimeSpan timeSpan) =>
    timeSpan.TotalSeconds > 1
        ? $"{timeSpan:s\\.ff}s"
        : $"{timeSpan:fff}ms";

View? GetActiveView() => leftPane.Subviews.SelectMany(v => v.Subviews).SingleOrDefault();

void KeyDownHandler(KeyEventEventArgs eventArgs)
{
    if (eventArgs.KeyEvent.Key == Key.Backspace && GetActiveView() == partList)
    {
        leftPane.RemoveAll();
        leftPane.Add(dayList);
        leftPane.Title = "Puzzles";
        dayList.SetFocus();
    }
    else if (digitKeyHandler.Handle(eventArgs.KeyEvent.Key) is { } number)
    {
        if (GetActiveView() == dayList && number > 0 && number <= days.Count)
        {
            dayList.SelectedItem = number - 1;
            dayList.SetNeedsDisplay();
        }
        else if (GetActiveView() == partList && number < parts.Count)
        {
            partList.SelectedItem = number;
            partList.SetNeedsDisplay();
        }
    }
}

async Task<(string result, TimeSpan time)> Measure(Func<Task<string>> func)
{
    var s = Stopwatch.StartNew();
    var result = await func();
    var time = TimeSpan.FromMilliseconds(s.ElapsedMilliseconds);

    return (result, time);
}
