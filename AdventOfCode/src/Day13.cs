using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode;

public class Day13 : BaseDay {
    private readonly string _input;
    List<ClawGame> games;

    readonly record struct ClawGame {

        public readonly int AX;
        public readonly int AY;
        public readonly int BX;
        public readonly int BY;
        public readonly int PrizeX;
        public readonly int PrizeY;

        public ClawGame(int AX, int AY, int BX, int BY, int PrizeX, int PrizeY) {
            this.AX = AX;
            this.AY = AY;
            this.BX = BX;
            this.BY = BY;
            this.PrizeX = PrizeX;
            this.PrizeY = PrizeY;
        }
    }

    public Day13() {
        _input = File.ReadAllText(InputFilePath);
        games = [];
        string pattern = @"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)";
        foreach (var machine in _input.Split("\n\n")) {
            var words = Regex.Match(machine, pattern).Groups.Values;
            var vals = words.Skip(1).Select(x => Int32.Parse(x.ValueSpan)).ToArray();
            games.Add(new ClawGame(vals[0], vals[1], vals[2], vals[3], vals[4], vals[5]));
        }
    }

    public override ValueTask<string> Solve_1() {
        Dictionary<ClawGame, (int APress, int BPress)> winnable = [];
        foreach (var game in games)
        {
            // linear combination of AX and BX == PrizeX && AY and BY == PrizeY
            var A = Matrix<double>.Build.DenseOfArray(new double[,]{
                {game.AX, game.BX},
                {game.AY, game.BY}
            });
            var B = Vector<double>.Build.Dense([game.PrizeX, game.PrizeY]);
            var x = A.Solve(B);

            if(x[0] >= 0 && Math.Abs(x[0]-Math.Round(x[0])) < (1E-10) && x[0] < 100 &&
                x[1] >= 0 && Math.Abs(x[1]- Math.Round(x[1])) < (1E-10) && x[1] < 100){
                winnable.Add(game, ((int)Math.Round(x[0]), (int)Math.Round(x[1])));
            }
        }
        var tokens = 0;
        foreach(var (game, presses) in winnable){
            tokens += presses.APress * 3;
            tokens += presses.BPress;
        }
        return new ValueTask<string>($"{tokens}");
    }

    public override ValueTask<string> Solve_2() {
        Dictionary<ClawGame, (long APress, long BPress)> winnable = [];
        foreach (var game in games)
        {
            // linear combination of AX and BX == PrizeX && AY and BY == PrizeY
            var A = Matrix<double>.Build.DenseOfArray(new double[,]{
                {game.AX, game.BX},
                {game.AY, game.BY}
            });
            var B = Vector<double>.Build.Dense([game.PrizeX+10000000000000L, game.PrizeY+10000000000000L]);
            var x = A.Solve(B);

            if(x[0] >= 0 && Math.Abs(x[0]-Math.Round(x[0])) < (.001) && 
                x[1] >= 0 && Math.Abs(x[1]- Math.Round(x[1])) < (.001)){
                winnable.Add(game, ((long)Math.Round(x[0]), (long)Math.Round(x[1])));
            }
        }
        var tokens = 0L;
        foreach(var (game, presses) in winnable){
            tokens += presses.APress * 3L;
            tokens += presses.BPress;
        }
        return new ValueTask<string>($"{tokens}");
    }

}