using System.Text;

namespace AdventOfCode;

public class Day07 : BaseDay {
    private readonly string _input;
    List<Calibration> calibrations = [];
    private readonly struct Calibration {
        public long Target { get; init; }
        public List<int> Ints { get; init; }

        public Calibration(string line) {
            var words = line.Split(": ");
            Target = long.Parse(words[0]);
            Ints = words[1].Split(" ").Select(x => Int32.Parse(x)).ToList();
        }
    }

    public Day07() {
        _input = File.ReadAllText(InputFilePath);
        foreach (var line in _input.Split("\n")) {
            calibrations.Add(new Calibration(line));
        }
    }

    public override ValueTask<string> Solve_1() {
        var sum = 0L;
        foreach (var cal in calibrations) {
            var all = 1 << (cal.Ints.Count - 1);
            for (int i = 0; i <= all; i++) {
                if (Evaluate(cal.Ints, i) == cal.Target) {
                    sum += cal.Target;
                    break;
                }
            }
        }
        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2() {
        var sum = 0L;
        foreach (var cal in calibrations) {
            var operators = cal.Ints.Count - 1;
            var all = (int)Math.Pow(3, operators);
            for (int i = 0; i < all; i++) {
                if (Evaluate3(cal.Ints, i) == cal.Target) {
                    sum += cal.Target;
                    break;
                }
            }
        }
        return new ValueTask<string>($"{sum}");
    }

    private string ToBase3(int num, int length) {
        StringBuilder builder = new StringBuilder(length);

        while (builder.Length < length) {
            builder.Append('0');
        }
        return builder.ToString();
    }

    private long Evaluate(List<int> numbers, int bitset) {
        long temp = numbers[0];
        for (int i = 1; i < numbers.Count; i++) {
            if ((bitset & (1 << i - 1)) == 0) {
                temp += numbers[i];
            } else {
                temp *= numbers[i];
            }
        }
        return temp;
    }

    private long Evaluate3(List<int> numbers, int bitset) {
        long temp = numbers[0];
        for (int i = 1; i < numbers.Count; i++) {
            int op = bitset % 3;
            bitset /= 3;
            switch (op) {
                case 0:
                    temp += numbers[i];
                    break;
                case 1:
                    temp *= numbers[i];
                    break;
                case 2:
                    var n = numbers[i];
                    while (n > 0) {
                        temp *= 10;
                        n /= 10;
                    }
                    temp += numbers[i];
                    break;
                default:
                    Console.WriteLine("Error, invalid op");
                    break;
            }
        }
        return temp;
    }

}