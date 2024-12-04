using Spectre.Console;

if (args.Length == 0)
{
    await Solver.SolveLast(opt => opt.ClearConsole = false);
}
else if (args.Length == 1 && args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase))
{
    Console.WriteLine(Environment.GetEnvironmentVariable("GITHUB_ACTION"));
    if(Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true"){
        AnsiConsole.Record();
    }
    await Solver.SolveAll(opt =>
    {
        opt.ShowConstructorElapsedTime = true;
        opt.ShowTotalElapsedTimePerDay = true;
        opt.ShowOverallResults = false;
        opt.ClearConsole = true;
    });
    if(Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true"){
        string text = AnsiConsole.ExportHtml();
        File.WriteAllText("results_log", text);
    }
}
else
{
    var indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);

    await Solver.Solve(indexes.Where(i => i < uint.MaxValue));
}
