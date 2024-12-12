using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day03 : BaseDay {
    private readonly string _input;

    public Day03() {
        _input = File.ReadAllText(InputFilePath);

    }

    public override ValueTask<string> Solve_1() {
        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        var sum = 0;
        foreach (Match match in Regex.Matches(_input, pattern)) {
            sum += Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
        }
        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2() {
        string pattern = @"(mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\))";
        var sum = 0;
        bool enabled = true;
        foreach (Match match in Regex.Matches(_input, pattern)) {
            if (match.Value == "do()") {
                enabled = true;
            } else if (match.Value == "don't()") {
                enabled = false;
            } else if (enabled) {
                sum += Int32.Parse(match.Groups[2].Value) * Int32.Parse(match.Groups[3].Value);
            }
        }
        return new ValueTask<string>(sum.ToString());
    }

}