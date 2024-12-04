namespace AdventOfCode;

public class Grid {
    public char[,] state;
    public int Width {get; private set;}
    public int Height {get; private set;}

    public static readonly (int, int) DirUp = (-1,0);
    public static readonly (int, int) DirUpRight = (-1,1);
    public static readonly (int, int) DirRight = (0,1);
    public static readonly (int, int) DirDownRight = (1,1);
    public static readonly (int, int) DirDown = (1,0);
    public static readonly (int, int) DirDownLeft = (1,-1);
    public static readonly (int, int) DirLeft = (0,-1);
    public static readonly (int, int) DirUpLeft = (-1,-1);

    public static readonly (int,int)[] Diagonals = [DirUpRight, DirDownRight, DirDownLeft, DirUpLeft];
    public static readonly (int,int)[] Ortho = [DirUp, DirRight, DirDown, DirLeft];
    public static readonly (int,int)[] AllDirs = [DirUp, DirUpRight, DirRight, DirDownRight, DirDown, DirDownLeft, DirLeft, DirUpLeft];

    public Grid(int width, int height)
    {
        state = new char[width, height];
        Width = width;
        Height = height;
    }

    public void Fill(string[] lines) {
        if(lines[0].Length != Width || lines.Length != Height) {
            throw new InvalidDataException("Grid data size mismatch");
        }
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                state[i, j] = line[j];
            }
        }
    }

    public bool MatchDir((int y,int x) coord, char[] target, (int dy, int dx) dir){
        if((target.Length*dir.dy) + coord.y > state.GetLength(0) || (target.Length*dir.dy) + coord.y < -1) {
            return false;
        }
        if((target.Length*dir.dx) + coord.x > state.GetLength(1) || (target.Length*dir.dx) + coord.x < -1) {
            return false;
        }
        for (int i = 0; i < target.Length; i++)
        {
            if(state[coord.y+(i*dir.dy), coord.x+(i*dir.dx)] != target[i]){
                return false;
            }
        }
        return true;
    }

}

public class Day04 : BaseDay
{
    private readonly string _input;
    private Grid grid;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
        var lines = _input.Split("\n")[..^1];
        grid = new Grid(lines[0].Length, lines.Length);
        grid.Fill(lines);
    }

    public override ValueTask<string> Solve_1()
    {
        char[] target = ['X', 'M', 'A', 'S'];
        var dirs = Grid.AllDirs;
        var count = 0;
        for (int i = 0; i < grid.Height; i++)
        {
            for (int j = 0; j < grid.Width; j++)
            {
                for (int k = 0; k < dirs.Length; k++)
                {
                    if(grid.state[i,j] == target[0] && grid.MatchDir((i, j), target, dirs[k])){
                        count += 1;
                    }
                }
            }
        }
        return new ValueTask<string>(count.ToString());
    }

    public override ValueTask<string> Solve_2()  
    {
        char[] target = ['M', 'A', 'S'];
        var dirs = Grid.Diagonals;
        var count = 0;
        var centers = new Dictionary<(int,int), int>();
        for (int i = 0; i < grid.Height; i++)
        {
            for (int j = 0; j < grid.Width; j++)
            {
                for (int k = 0; k < dirs.Length; k++)
                {
                    if(grid.state[i,j] == target[0] && grid.MatchDir((i, j), target, dirs[k])){
                        var center = (i+dirs[k].Item1,j+dirs[k].Item2);
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