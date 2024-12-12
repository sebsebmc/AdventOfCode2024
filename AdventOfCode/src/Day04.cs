namespace AdventOfCode;

public class Day04 : BaseDay {
    private readonly string _input;
    private Grid grid;

    public Day04() {
        _input = File.ReadAllText(InputFilePath);
        var lines = _input.Split("\n")[..^1];
        grid = new Grid(lines[0].Length, lines.Length);
        grid.Fill(lines);
    }

    public override ValueTask<string> Solve_1() {
        char[] target = ['X', 'M', 'A', 'S'];
        var dirs = Grid.AllDirs;
        var count = 0;
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                for (int k = 0; k < dirs.Length; k++) {
                    if (grid.state[i, j] == target[0] && grid.MatchDir((i, j), target, dirs[k])) {
                        count += 1;
                    }
                }
            }
        }
        return new ValueTask<string>(count.ToString());
    }

    public override ValueTask<string> Solve_2() {
        char[] target = ['M', 'A', 'S'];
        var dirs = Grid.Diagonals;
        var count = 0;
        var centers = new Dictionary<(int, int), int>();
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                for (int k = 0; k < dirs.Length; k++) {
                    if (grid.state[i, j] == target[0] && grid.MatchDir((i, j), target, dirs[k])) {
                        var center = (i + dirs[k].Item1, j + dirs[k].Item2);
                        var centered = centers.GetValueOrDefault(center, 0);
                        centers[center] = centered + 1;
                    }
                }
            }
        }
        count = centers.Count(p => p.Value == 2);
        return new ValueTask<string>(count.ToString());
    }

}