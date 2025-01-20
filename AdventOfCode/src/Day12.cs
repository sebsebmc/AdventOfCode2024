
namespace AdventOfCode;

public class Day12 : BaseDay {
    private readonly string _input;
    Grid grid;

    public Day12() {
        _input = File.ReadAllText(InputFilePath);
        var lines = _input.Split("\n");
        grid = new Grid(lines.Length, lines[0].Length);
        grid.Fill(lines);
    }

    public override ValueTask<string> Solve_1() {
        var lookup = new Dictionary<Coordinate, HashSet<Coordinate>>();


        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                char curReg = grid.state[i, j];
                var coord = new Coordinate(i, j);
                HashSet<Coordinate> lr = null, ar = null;
                // do we belong to above region?
                if (i != 0 && grid.state[i - 1, j] == curReg) {
                    ar = lookup[new Coordinate(i - 1, j)];
                    ar.Add(coord);
                    lookup[coord] = ar;
                }
                //do we belong to left region?
                if (j != 0 && grid.state[i, j - 1] == curReg) {
                    lr = lookup[new Coordinate(i, j - 1)];
                    // do we need to merge left and above region?
                    if (ar != null && ar != lr) {
                        // merge
                        // System.Console.WriteLine($"Merging region {curReg} of {ar.Count} and {lr.Count}");
                        MergeAndUpdate(lookup, ar, lr);
                    } else {
                        lr.Add(coord);
                        lookup[coord] = lr;
                    }
                }

                if (lr == null && ar == null) {
                    var newRegion = new HashSet<Coordinate>();
                    newRegion.Add(coord);
                    lookup[coord] = newRegion;
                    // Console.Write("X");
                }
            }
            // Console.WriteLine();
        }
        var sum = 0;
        foreach (var reg in lookup.Values.Distinct()) {
            var fence = 0;
            foreach (var coord in reg) {
                var neighbors = 0;
                foreach (var dir in Grid.Ortho) {
                    if (grid.IsValid(coord, dir) && grid.At(coord, dir) == grid.At(coord)) {
                        neighbors += 1;
                    }
                }
                fence += 4 - neighbors;
            }
            // System.Console.WriteLine($"A region of {grid.At(reg.First())} plants with price {reg.Count} * {fence}");
            sum += reg.Count * fence;
        }
        return new ValueTask<string>($"{sum}");
    }

    private void MergeAndUpdate(Dictionary<Coordinate, HashSet<Coordinate>> lookup, HashSet<Coordinate> ar, HashSet<Coordinate> lr) {
        ar.UnionWith(lr);
        foreach (var coord in lr) {
            lookup[coord] = ar;
        }
    }

    int AddDir((int dy, int dx) dir, int mask) {
        return dir switch {
            var t when dir == Grid.DirUp => mask | 0b1000,
            var t when dir == Grid.DirRight => mask | 0b0100,
            var t when dir == Grid.DirDown => mask | 0b0010,
            var t when dir == Grid.DirLeft => mask | 0b0001,
            _ => mask,
        };
    }

    int ClearDir((int dy, int dx) dir, int mask) {
        return dir switch {
            var t when dir == Grid.DirUp => mask & ~0b1000,
            var t when dir == Grid.DirRight => mask & ~0b0100,
            var t when dir == Grid.DirDown => mask & ~0b0010,
            var t when dir == Grid.DirLeft => mask & ~0b0001,
            _ => mask,
        };
    }

    bool CheckDir((int dy, int dx) dir, int mask) {
        return dir switch {
            var t when dir == Grid.DirUp => (mask & 0b1000) != 0,
            var t when dir == Grid.DirRight => (mask & 0b0100) != 0,
            var t when dir == Grid.DirDown => (mask & 0b0010) != 0,
            var t when dir == Grid.DirLeft => (mask & 0b0001) != 0,
            _ => false,
        };
    }

    public override ValueTask<string> Solve_2() {
        var regions = new List<HashSet<Coordinate>>();
        var lookup = new Dictionary<Coordinate, HashSet<Coordinate>>();
        var slookup = new Dictionary<Coordinate, int>();


        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                char curReg = grid.state[i, j];
                var coord = new Coordinate(i, j);
                HashSet<Coordinate> lr = null, ar = null;
                // do we belong to above region?
                if (i != 0 && grid.state[i - 1, j] == curReg) {
                    ar = lookup[new Coordinate(i - 1, j)];
                    ar.Add(coord);
                    lookup[coord] = ar;
                }
                //do we belong to left region?
                if (j != 0 && grid.state[i, j - 1] == curReg) {
                    lr = lookup[new Coordinate(i, j - 1)];
                    // do we need to merge left and above region?
                    if (ar != null && ar != lr) {
                        // merge
                        MergeAndUpdate(lookup, ar, lr);
                    } else {
                        lr.Add(coord);
                        lookup[coord] = lr;
                    }
                }

                if (lr == null && ar == null) {
                    var newRegion = new HashSet<Coordinate>();
                    newRegion.Add(coord);
                    lookup[coord] = newRegion;
                }
            }
        }

        foreach (var reg in lookup.Values.Distinct()) {
            foreach (var coord in reg) {
                foreach (var dir in Grid.Ortho) {
                    if (grid.IsValid(coord, dir)) {
                        if(grid.At(coord, dir) != grid.At(coord)) {
                            slookup[coord] = AddDir(dir, slookup.GetValueOrDefault(coord, 0));
                        }
                    }else { // Add outside borders
                        slookup[coord] = AddDir(dir, slookup.GetValueOrDefault(coord, 0));
                    }
                }
            }
        }
        // If we really wanted to we can merge the above with part 1 and reuse all of it in part 2

        var sum = 0;
        foreach (var reg in lookup.Values.Distinct()) {
            var sides = 0;
            while (reg.Any(x => slookup.GetValueOrDefault(x, 0) != 0)) {
                var start = reg.First(x => slookup.GetValueOrDefault(x, 0) != 0);
                var startDir = Grid.RotateRight(Grid.Ortho.First(x => CheckDir(x, slookup[start])));
                Coordinate cur = start;
                var dir = startDir;

                do {
                    var wall = Grid.RotateLeft(dir);
                    slookup[cur] = ClearDir(wall, slookup[cur]);
                    if (grid.IsValid(cur, dir)) {
                        if (grid.At(cur, dir) == grid.At(cur)) {
                            var ahead = new Coordinate(cur.Y + dir.dy, cur.X + dir.dx);
                            var left = Grid.RotateLeft(dir);
                            // Check if we need to look around a corner to the left
                            if (grid.IsValid(ahead, left) && grid.At(ahead) == grid.At(ahead, left)) {
                                dir = left;
                                var lcoord = new Coordinate(ahead.Y + dir.dy, ahead.X + dir.dx);
                                sides += 1;
                                cur = lcoord;
                            } else {
                                cur = ahead;
                            }
                        } else {
                            dir = Grid.RotateRight(dir);
                            sides += 1;
                        }
                    } else {
                        dir = Grid.RotateRight(dir);
                        sides += 1;
                    }

                } while (!(cur == start && dir == startDir));
            }

            sum += reg.Count * sides;
        }
        return new ValueTask<string>($"{sum}");
    }

    private void PrintGridDict(Dictionary<Coordinate, int> slookup) {
        for (int i = 0; i < grid.Height; i++) {
            for (int j = 0; j < grid.Width; j++) {
                Console.Write($"{slookup.GetValueOrDefault(new Coordinate(i, j)),2}");
            }
            System.Console.WriteLine();
        }
    }

    static (Coordinate, Coordinate) ToInterior(Coordinate coord) {
        if (coord.Y % 2 == 0) { // horizontal boundaries (top/bottom)
            return (new Coordinate((coord.Y / 2) - 1, coord.X), new Coordinate(coord.Y / 2, coord.X));
        } else { // vertical boundaries (left/right)
            return (new Coordinate(coord.Y / 2, coord.X - 1), new Coordinate(coord.Y, coord.X));
        }
    }

}