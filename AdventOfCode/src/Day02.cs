using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public class Report {
    
    public List<int> Levels {get; set;} = [];
    
    public Report(List<int> levels){
        Levels = levels;
    }
    
    public Report(string line) {
        var words = line.Split(" ");
        foreach (var word in words)
        {
            Levels.Add(Int32.Parse(word));
        }
    }

    public Report Dampened(int index) {
        var newLevels = new List<int>(Levels);
        newLevels.RemoveAt(index);
        return new Report(newLevels);
    }


}

public class Day02 : BaseDay
{
    private readonly string _input;
    private readonly List<Report> _reports;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
        var lines = _input.Split("\n")[..^1];
        _reports = [];
        foreach (var line in lines)
        {
            _reports.Add(new Report(line));
        }

    }

    private bool IsReportSafe(Report report) {
        return report.Levels.Skip(1).Zip(report.Levels).All((pair) => pair.First < pair.Second && pair.Second - pair.First <=3) 
            || report.Levels.Skip(1).Zip(report.Levels).All((pair) => pair.First > pair.Second && pair.First - pair.Second <=3);
    }

    public override ValueTask<string> Solve_1()
    {
        var safe = 0;
        foreach (var report in _reports)
        {
            if ( IsReportSafe(report)) {
                safe += 1;
            }
        }

        return new ValueTask<string>(safe.ToString());
    }

    public override ValueTask<string> Solve_2()  
    {
        var safe = 0;
        List<int> differences = [];
        foreach(var report in _reports) {
            if (IsReportSafe(report)) {
                safe += 1;
            }else{
                for (int i = 0; i < report.Levels.Count; i++)
                {
                    if(IsReportSafe(report.Dampened(i))) {
                        safe += 1;
                        break;
                    }
                }
            }
        }
        return new ValueTask<string>(safe.ToString());
    }

}