namespace AdventOfCode;

using Direction = (int dy, int dx);

public class Grid {
    public char[,] state;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public static readonly Direction DirUp = (-1, 0);
    public static readonly Direction DirUpRight = (-1, 1);
    public static readonly Direction DirRight = (0, 1);
    public static readonly Direction DirDownRight = (1, 1);
    public static readonly Direction DirDown = (1, 0);
    public static readonly Direction DirDownLeft = (1, -1);
    public static readonly Direction DirLeft = (0, -1);
    public static readonly Direction DirUpLeft = (-1, -1);

    public static readonly Direction[] Diagonals = [DirUpRight, DirDownRight, DirDownLeft, DirUpLeft];
    public static readonly Direction[] Ortho = [DirUp, DirRight, DirDown, DirLeft];
    public static readonly Direction[] AllDirs = [DirUp, DirUpRight, DirRight, DirDownRight, DirDown, DirDownLeft, DirLeft, DirUpLeft];

    public Grid(int width, int height) {
        state = new char[height, width];
        Width = width;
        Height = height;
    }

    public void Fill(string[] lines) {
        if (lines[0].Length != Width || lines.Length != Height) {
            throw new InvalidDataException("Grid data size mismatch");
        }
        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++) {
                state[i, j] = line[j];
            }
        }
    }

    public bool MatchDir((int y, int x) coord, char[] target, Direction dir) {
        if ((target.Length * dir.dy) + coord.y > state.GetLength(0) || (target.Length * dir.dy) + coord.y < -1) {
            return false;
        }
        if ((target.Length * dir.dx) + coord.x > state.GetLength(1) || (target.Length * dir.dx) + coord.x < -1) {
            return false;
        }
        for (int i = 0; i < target.Length; i++) {
            if (state[coord.y + (i * dir.dy), coord.x + (i * dir.dx)] != target[i]) {
                return false;
            }
        }
        return true;
    }

    public bool IsValid((int y, int x) coord) {
        return coord.y >= 0 && coord.x >= 0 && coord.y < this.Height && coord.x < this.Width;
    }

    public bool IsValid((int y, int x) coord, Direction dir) {
        return coord.y + dir.dy >= 0 && coord.x + dir.dx >= 0 &&
            coord.y + dir.dy < this.Height && coord.x + dir.dx < this.Width;
    }

    public static Direction RotateRight(Direction dir) {
        for (int i = 0; i < AllDirs.Length; i++) {
            if (AllDirs[i] == dir) {
                return AllDirs[(i + 2) % AllDirs.Length];
            }
        }
        throw new InvalidDataException("Not a valid direction");
    }

    public void Print() {
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                Console.Write(state[i, j]);
            }
            Console.WriteLine();
        }
    }

}