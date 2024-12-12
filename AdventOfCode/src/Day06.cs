namespace AdventOfCode;

public class Day06 : BaseDay {
    private readonly string _input;
    Grid grid;

    private (int row, int col) position = (0, 0);
    private string[] lines;

    public Day06() {
        _input = File.ReadAllText(InputFilePath);
        lines = _input.Split("\n");

    }

    public override ValueTask<string> Solve_1() {
        grid = new Grid(lines[0].Length, lines.Length);
        grid.Fill(lines);
        var idx = _input.IndexOf('^');
        position = (idx / (grid.Width + 1), idx % (grid.Width + 1));

        var dir = Grid.DirUp;
        do {
            while (grid.IsValid(position, dir) && grid.state[position.row + dir.dy, position.col + dir.dx] == '#') {
                dir = Grid.RotateRight(dir);
            }
            grid.state[position.row, position.col] = 'X';
            position = (position.row + dir.dy, position.col + dir.dx);
        } while (grid.IsValid(position));

        var total = 0;
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                // Console.Write(grid.state[i,j]);
                if (grid.state[i, j] == 'X') {
                    total += 1;
                }
            }
            // Console.WriteLine();
        }
        return new ValueTask<string>($"{total}");
    }

    Dictionary<(int, int), int> dirmap = new Dictionary<(int, int), int>{{Grid.DirUp, 0b0001},
        {Grid.DirRight, 0b0010},
        {Grid.DirDown, 0b0100}, {Grid.DirLeft, 0b1000}};
    private char AddDir(char c, (int, int) d) {
        return (char)(c | dirmap[d]);
    }

    private bool CheckDir(char c, (int, int) d) {
        return (c & dirmap[d]) != 0;
    }

    public override ValueTask<string> Solve_2() {
        return new ValueTask<string>("Skipped"); // I need to actually do a walk for each possible new obstacle and make a deep copy of the map to do so
        grid = new Grid(lines[0].Length, lines.Length);
        grid.Fill(lines);
        var idx = _input.IndexOf('^');
        position = (idx / (grid.Width + 1), idx % (grid.Width + 1));
        var startPosition = (idx / (grid.Width + 1), idx % (grid.Width + 1));
        grid.state[position.row, position.col] = '0';
        Console.WriteLine($"Start position {startPosition}");

        var dir = Grid.DirUp;
        var total = 0;
        bool check1wide = false;
        do {
            grid.state[position.row, position.col] = AddDir(grid.state[position.row, position.col], dir);
            char previous = grid.state[position.row, position.col];
            if (grid.IsValid(position, dir) && grid.state[position.row + dir.dy, position.col + dir.dx] == '#') {
                dir = Grid.RotateRight(dir);
                grid.state[position.row, position.col] = AddDir(grid.state[position.row, position.col], dir);
                check1wide = false;
            }
            // If we need to rotate again we need to later check for a 1-wide loop
            if (grid.IsValid(position, dir) && grid.state[position.row + dir.dy, position.col + dir.dx] == '#') {
                dir = Grid.RotateRight(dir);
                grid.state[position.row, position.col] = AddDir(grid.state[position.row, position.col], dir);
                check1wide = true;
            }

            var scanDir = Grid.RotateRight(dir);
            (int y, int x) scanPos = (position.row + scanDir.dy, position.col + scanDir.dx);
            while (grid.IsValid(scanPos) && grid.state[scanPos.y, scanPos.x] != '#') {
                previous = grid.state[scanPos.y, scanPos.x];
                scanPos = (scanPos.y + scanDir.dy, scanPos.x + scanDir.dx);
            }
            var nextPos = (position.row + dir.dy, position.col + dir.dx);
            // Dont need to check if we also check IsValid grid.state[scanPos.y, scanPos.x] == '#'
            if (grid.IsValid(nextPos) && nextPos != startPosition //checking if we can place an obstacle
                && grid.IsValid(scanPos)) //checking if it makes us loop
            {
                //&& CheckDir(previous, Grid.RotateRight(scanDir)
                if (CheckDir(previous, Grid.RotateRight(scanDir))) {
                    Console.WriteLine($"Loop found with obstacle into {previous} at {position}");
                    total += 1;
                } else if (check1wide) {
                    Console.WriteLine($"Loop found with obstacle into {previous} at {position}");
                    total += 1;
                    // if we have a 1-wide box then we need to chack if rotating and scanning closes the loop
                }
            }
            position = nextPos;
            grid.Print();
            Console.WriteLine();
        } while (grid.IsValid(position));
        return new ValueTask<string>($"{total}");
    }

}