namespace AdventOfCode;

public class Day10 : BaseDay {
    private readonly string _input;

    Grid grid;

    public Day10() {
        _input = File.ReadAllText(InputFilePath);
        var lines = _input.Split("\n");
        grid = new Grid(lines.Length, lines[0].Length);
        grid.Fill(lines);
    }

    public override ValueTask<string> Solve_1() {
        var ths = grid.FindAll('0');
        var stack = new Stack<Coordinate>();

        int total = 0;
        foreach (var start in ths) {
            stack.Push(start);

            var visited = new HashSet<Coordinate>();
            while (stack.Count > 0) {
                var cur = stack.Pop();
                var next = grid.At(cur) + 1;

                if(visited.Contains(cur)){
                    continue;
                }
                visited.Add(cur);

                foreach (var dir in Grid.Ortho) {
                    Coordinate nCoord = new(cur.Y + dir.dy, cur.X + dir.dx);
                    if (grid.IsValid(nCoord)) {
                        if (grid.At(nCoord) == next) {
                            // System.Console.WriteLine($"{(char)next} at {nCoord}");
                            if (next == '9' && !visited.Contains(nCoord)) {
                                total += 1;
                                visited.Add(nCoord);
                            } else {
                                stack.Push(nCoord);
                            }
                        }
                    }
                }
                
            }
        }

        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2() {
        var ths = grid.FindAll('0');
        var stack = new Stack<Coordinate>();

        int total = 0;
        foreach (var start in ths) {
            stack.Push(start);

            while (stack.Count > 0) {
                var cur = stack.Pop();
                var next = grid.At(cur) + 1;

                foreach (var dir in Grid.Ortho) {
                    Coordinate nCoord = new(cur.Y + dir.dy, cur.X + dir.dx);
                    if (grid.IsValid(nCoord)) {
                        if (grid.At(nCoord) == next) {
                            // System.Console.WriteLine($"{(char)next} at {nCoord}");
                            if (next == '9') {
                                total += 1;
                            } else {
                                stack.Push(nCoord);
                            }
                        }
                    }
                }
                
            }
        }

        return new ValueTask<string>($"{total}");
    }

}