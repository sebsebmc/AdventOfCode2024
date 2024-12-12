namespace AdventOfCode;

public class Day08 : BaseDay {
    private readonly string _input;
    private SparseMap map;

    class SparseMap {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Dictionary<string, List<(int x, int y)>> Locations = [];

        public SparseMap(string text) {
            var lines = text.Split("\n");
            Height = lines.Length;
            Width = lines[0].Length;
            for (int y = 0; y < lines.Length; y++) {
                string line = lines[y];
                for (int x = 0; x < line.Length; x++) {
                    char c = line[x];
                    if (c == '.') {
                        continue;
                    }
                    var key = c.ToString();
                    var l = Locations.GetValueOrDefault(key, new List<(int x, int y)>());
                    l.Add((x, y));
                    Locations[key] = l;
                }
            }
        }

        public bool IsValid((int x, int y) coord) {
            return coord.x >= 0 && coord.y >= 0 && coord.x < Width && coord.y < Height;
        }
    }

    public Day08() {
        _input = File.ReadAllText(InputFilePath);
        map = new SparseMap(_input);
    }

    static IEnumerable<(T, T)> Combinations<T>(IEnumerable<T> list) {
        if (list.Count() < 2) {
            yield break;
        }
        for (int i = 0; i < list.Count() - 1; i++) {
            for (int j = i + 1; j < list.Count(); j++) {
                yield return (list.ElementAt(i), list.ElementAt(j));
            }
        }
        yield break;
    }

    public override ValueTask<string> Solve_1() {
        HashSet<(int x, int y)> set = [];
        foreach (var (letter, coords) in map.Locations) {
            foreach (var (fst, snd) in Combinations<(int x, int y)>(coords)) {
                (int dx, int dy) diff = (snd.x - fst.x, snd.y - fst.y);
                var p1 = (fst.x - diff.dx, fst.y - diff.dy);
                var p2 = (snd.x + diff.dx, snd.y + diff.dy);
                if (map.IsValid(p1)) {
                    set.Add(p1);
                }
                if (map.IsValid(p2)) {
                    set.Add(p2);
                }
            }
        }
        return new ValueTask<string>($"{set.Count}");
    }

    public override ValueTask<string> Solve_2() {
        HashSet<(int x, int y)> set = [];
        foreach (var (letter, coords) in map.Locations) {
            if (coords.Count == 1) {
                continue;
            }
            set.UnionWith(coords);
            foreach (var (fst, snd) in Combinations<(int x, int y)>(coords)) {
                (int dx, int dy) diff = (snd.x - fst.x, snd.y - fst.y);
                var p1 = (fst.x - diff.dx, fst.y - diff.dy);
                var p2 = (snd.x + diff.dx, snd.y + diff.dy);
                while (map.IsValid(p1)) {
                    set.Add(p1);
                    p1 = (p1.Item1 - diff.dx, p1.Item2 - diff.dy);
                }
                while (map.IsValid(p2)) {
                    set.Add(p2);
                    p2 = (p2.Item1 + diff.dx, p2.Item2 + diff.dy);
                }
            }
        }
        return new ValueTask<string>($"{set.Count}");
    }

}