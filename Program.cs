using AoC_2021;
using Microsoft.Extensions.Configuration;
using Terminal.Gui;
using static System.Environment;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var sessionId = config["SessionId"];

Application.Init();

List<IDay> days = new()
{
    new Day1(sessionId),
    new Day2(sessionId)
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

// ReSharper disable once AsyncVoidLambda
dayList.OpenSelectedItem += async eventArgs =>
{
    var day = (IDay)eventArgs.Value;
    outputView.Text += $" {day.Name}, Part One: {await day.PartOne()}{NewLine}";
    outputView.Text += $" {day.Name}, Part Two: {await day.PartTwo()}{NewLine}";
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

Application.Top.Add(title);
Application.Top.Add(leftPane);
Application.Top.Add(rightPane);
Application.Top.Add(statusBar);

Application.Run();
