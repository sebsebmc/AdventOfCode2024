using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day14 : BaseDay {
    private readonly string _input;

    record Robot(int X, int Y, int Dx, int Dy);
    List<Robot> robots;

    int WIDTH = 101;
    int HEIGHT = 103;

    public Day14() {
        robots = [];
        _input = File.ReadAllText(InputFilePath);
        string pattern = @"p=([\-\d]+),([\-\d]+) v=([\-\d]+),([\-\d]+)";
        foreach (var line in _input.Split("\n")) {
            var words = Regex.Match(line, pattern).Groups.Values;
            var vals = words.Skip(1).Select(x => Int32.Parse(x.ValueSpan)).ToArray();
            robots.Add(new Robot(vals[0], vals[1], vals[2], vals[3]));
        }

        // For test cases
        if (robots.Count < 20) {
            WIDTH = 11;
            HEIGHT = 7;
        }

    }

    public override ValueTask<string> Solve_1() {
        int[] quadrants = [0, 0, 0, 0];
        foreach (var robot in robots) {
            var finalX = ((robot.X + (robot.Dx * 100)) % WIDTH + WIDTH) % WIDTH;
            var finalY = ((robot.Y + (robot.Dy * 100)) % HEIGHT + HEIGHT) % HEIGHT;
            if (finalX < WIDTH / 2) {
                if (finalY < HEIGHT / 2) {
                    quadrants[0] += 1;
                } else if (finalY > HEIGHT / 2) {
                    quadrants[2] += 1;
                }
            } else if (finalX > WIDTH / 2) {
                if (finalY < HEIGHT / 2) {
                    quadrants[1] += 1;
                } else if (finalY > HEIGHT / 2) {
                    quadrants[3] += 1;
                }
            }
        }
        return new ValueTask<string>($"{quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3]}");
    }

    public override ValueTask<string> Solve_2() {
        // Assuming that the arrangement means no overlapping robots, we need to find a value of t such that
        // r.x + (r.dx * t) % width and r.y + (r.dy * t) % height all produce unique values
        Dictionary<Coordinate, List<Robot>> locations = [];
        Dictionary<Coordinate, List<Robot>> temp = [];

        foreach (var robot in robots) {
            locations[new Coordinate(robot.Y, robot.X)] = [robot];
        }
        var steps = 0;
        while (true) {
            var overlapping = 0;
            foreach (var (coord, robots) in locations) {
                foreach (var robot in robots) {
                    var finalX = ((coord.X + (robot.Dx)) % WIDTH + WIDTH) % WIDTH;
                    var finalY = ((coord.Y + (robot.Dy)) % HEIGHT + HEIGHT) % HEIGHT;
                    var ncoord = new Coordinate(finalY, finalX);
                    var l = temp.GetValueOrDefault(ncoord, []);
                    if (l.Count > 0) {
                        overlapping += 1;
                    }
                    l.Add(robot);
                    temp[ncoord] = l;
                }
            }
            steps += 1;
            locations = temp;
            temp = [];
            if (overlapping == 0) {
                return new ValueTask<string>($"{steps}");
            }
        }
    }

}