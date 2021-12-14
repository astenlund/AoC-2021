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

Application.Init();

List<IDay> days = new()
{
    new Day1(sessionId),
    new Day2(sessionId),
    new Day3(sessionId),
    new Day4(sessionId),
    new Day5(sessionId),
    new Day6(sessionId),
    new Day7(sessionId),
    new Day8(sessionId),
    new Day9(sessionId),
    new Day10(sessionId),
    new Day11(sessionId),
    new Day12(sessionId),
    new Day13(sessionId),
    new Day14(sessionId)
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
            rightPane.Title = $"Output: {selectedDay} Part 1";
            outputView.Text = FormatOutput(await selectedDay!.PartOne());
            break;
        case 2:
            rightPane.Title = $"Output: {selectedDay} Part 2";
            outputView.Text = FormatOutput(await selectedDay!.PartTwo());
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

Application.Top.KeyDown += KeyDownHandler;

Application.Top.Add(title);
Application.Top.Add(leftPane);
Application.Top.Add(rightPane);
Application.Top.Add(statusBar);

Application.Run();

string FormatOutput(string output) => NewLine + " " + string.Join(NewLine + " ", Regex.Split(output, @"\r?\n"));

View? GetActiveView() => leftPane.Subviews.SelectMany(v => v.Subviews).SingleOrDefault();
