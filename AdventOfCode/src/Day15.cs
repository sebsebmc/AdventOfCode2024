namespace AdventOfCode;

public class Day15 : BaseDay {
    private readonly string _input;
    Grid grid;
    Grid grid2;
    String moves;

    const char WALL = '#';
    const char BOX = 'O';
    const char BOXL = '[';
    const char BOXR = ']';
    const char EMPTY = '.';
    const char ROBOT = '@';

    readonly Dictionary<char, (int dy, int dx)> dirMap = new() { ['<'] = (0, -1), ['v'] = (1, 0), ['^'] = (-1, 0), ['>'] = (0, 1) };

    public Day15() {
        _input = File.ReadAllText(InputFilePath);
        var parts = _input.Split("\n\n");
        var map = parts[0].Split("\n");
        grid = new Grid(map[0].Length, map.Length);
        grid.Fill(map);
        moves = parts[1].Replace("\n", "");

        grid2 = new Grid(map[0].Length * 2, map.Length);
        for (int i = 0; i < map.Length; i++) {
            for (int j = 0; j < map[0].Length; j++) {
                switch(map[i][j]){
                    case WALL:
                    grid2.state[i,j*2] = '#';
                    grid2.state[i,1+(j*2)] = '#';
                    break;
                    case ROBOT:
                    grid2.state[i,j*2] = '@';
                    grid2.state[i,1+(j*2)] = '.';
                    break;
                    case BOX:
                    grid2.state[i,j*2] = BOXL;
                    grid2.state[i,1+(j*2)] = BOXR;
                    break;
                    case EMPTY:
                    grid2.state[i,j*2] = EMPTY;
                    grid2.state[i,1+(j*2)] = EMPTY;
                    break;
                }
            }
        }
    }

    public override ValueTask<string> Solve_1() {
        Coordinate robot = new(0, 0);
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                if (grid.state[i, j] == ROBOT) {
                    robot = new Coordinate(i, j);
                }
            }
        }

        foreach (var move in moves) {
            var dir = dirMap[move];
            (var after, var length) = GetRun(grid, robot, dir);
            if (after == EMPTY) {
                // Move boxes, move the robot
                for (int i = length + 1; i > 1; i--) {
                    grid.state[robot.Y + (dir.dy * i), robot.X + (dir.dx * i)] = grid.state[robot.Y + (dir.dy * (i - 1)), robot.X + (dir.dx * (i - 1))];
                }
                grid.state[robot.Y, robot.X] = EMPTY;
                robot = new Coordinate(robot.Y + dir.dy, robot.X + dir.dx);
                grid.state[robot.Y, robot.X] = ROBOT;
            } else if (after == WALL) {
                // Do not move the robot, do not move boxes
            }

        }

        var score = 0;
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                if (grid.state[i, j] == BOX) {
                    score += (100 * i) + j;
                }
            }
        }
        return new ValueTask<string>($"{score}");
    }

    private void PrintGrid(Grid grid) {
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                System.Console.Write(grid.state[i, j]);
            }
            System.Console.WriteLine();
        }
    }

    (char next, int count) GetRun(Grid grid, Coordinate start, (int dy, int dx) dir) {
        var cur = new Coordinate(start.Y + dir.dy, start.X + dir.dx);
        var length = 0;
        while (grid.At(cur) == BOX || grid.At(cur) == BOXL || grid.At(cur) == BOXR) {
            length += 1;
            cur = new Coordinate(cur.Y + dir.dy, cur.X + dir.dx);
        }
        return (grid.At(cur), length);
    }

    public override ValueTask<string> Solve_2() {
        Coordinate robot = new(0, 0);
        for (int i = 0; i < grid2.Height; i++) {
            for (int j = 0; j < grid2.Width; j++) {
                if (grid2.state[i, j] == ROBOT) {
                    robot = new Coordinate(i, j);
                }
            }
        }

        foreach (var move in moves) {
            var dir = dirMap[move];
            if (dir == Grid.DirLeft || dir == Grid.DirRight) {
                (var after, var length) = GetRun(grid2, robot, dir);
                if (after == EMPTY) {
                    // Move boxes, move the robot
                    for (int i = length + 1; i > 1; i--) {
                        grid2.state[robot.Y + (dir.dy * i), robot.X + (dir.dx * i)] = grid2.state[robot.Y + (dir.dy * (i - 1)), robot.X + (dir.dx * (i - 1))];
                    }
                    grid2.state[robot.Y, robot.X] = EMPTY;
                    robot = new Coordinate(robot.Y + dir.dy, robot.X + dir.dx);
                    grid2.state[robot.Y, robot.X] = ROBOT;
                }
            } else {
                (var canMove, var toMove) = GetRunWide(grid2, robot, dir);
                if(canMove){
                    toMove.Sort((a, b) => -1*dir.dy*(a.Y - b.Y));

                    foreach (var box in toMove) {
                        var temp = grid2.state[box.Y + dir.dy, box.X + dir.dx];
                        grid2.state[box.Y + dir.dy, box.X + dir.dx] = grid2.At(box);
                        grid2.state[box.Y, box.X] = temp;
                    }
                    grid2.state[robot.Y, robot.X] = EMPTY;
                    robot = new Coordinate(robot.Y + dir.dy, robot.X + dir.dx);
                    grid2.state[robot.Y, robot.X] = ROBOT;
                }
            }
        }

        var score = 0;
        for (int i = 0; i < grid2.Height; i++) {
            for (int j = 0; j < grid2.Width; j++) {
                if (grid2.state[i, j] == BOXL) {
                    score += (100 * i) + j;
                }
            }
        }
        return new ValueTask<string>($"{score}");
    }

    (bool canMove, List<Coordinate>) GetRunWide(Grid grid, Coordinate start, (int dy, int dx) dir) {
        HashSet<Coordinate> moveable = [];
        Queue<Coordinate> queue = [];
        queue.Enqueue(new Coordinate(start.Y + dir.dy, start.X + dir.dx));

        while (queue.Count != 0){
            var cur = queue.Dequeue();
            if(moveable.Contains(cur)){
                continue;
            }
            switch (grid.At(cur)) {
                case BOXR:
                    moveable.Add(cur);
                    moveable.Add(cur with {X=cur.X -1});
                    queue.Enqueue(new Coordinate(cur.Y + dir.dy, cur.X - 1));
                    queue.Enqueue(new Coordinate(cur.Y + dir.dy, cur.X));
                    break;
                case BOXL:
                    moveable.Add(cur);
                    moveable.Add(cur with {X=cur.X + 1});
                    queue.Enqueue(new Coordinate(cur.Y + dir.dy, cur.X + 1));
                    queue.Enqueue(new Coordinate(cur.Y + dir.dy, cur.X));
                    break;
                case WALL:
                    return (false, []);
            }
        }
        return (true, moveable.ToList());
    }

}