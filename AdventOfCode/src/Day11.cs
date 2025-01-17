namespace AdventOfCode;

public class Day11 : BaseDay {
    private readonly string _input;
    LinkedList<long> stones;

    public Day11() {
        _input = File.ReadAllText(InputFilePath);
        stones = new();
        foreach(var num in _input.Split(" ")){
            stones.AddLast(Int64.Parse(num));
        }
    }


    // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
    // If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. 
    //   The left half of the digits are engraved on the new left stone, and the right half of the digits are
    //   engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 
    //   1000 would become stones 10 and 0.)
    // If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied
    //   by 2024 is engraved on the new stone.

    public override ValueTask<string> Solve_1() {
        var cur = stones.ToDictionary(x => x, x => 1);
        var next = new Dictionary<long, int>();

        for (int i = 0; i < 25; i++)
        {
            foreach(var (num, count) in cur){
                var str = num.ToString();
                if(num == 0){
                    next[1] = next.GetValueOrDefault(1, 0) + count;
                }else if (str.Length % 2 == 0)
                {
                    var fh = Int64.Parse(str[..(str.Length/2)]);
                    var sh = Int64.Parse(str[(str.Length/2)..]);
                    next[fh] = next.GetValueOrDefault(fh, 0) + count;
                    next[sh] = next.GetValueOrDefault(sh, 0) + count;
                }else {
                    next[num * 2024] = next.GetValueOrDefault(num * 2024, 0) + count;
                }
            }
            cur = next;
            next = new Dictionary<long, int>();   
        }

        return new ValueTask<string>($"{cur.Sum(x => x.Value)}");
    }

    // If we simply keep counts of the number of each type of stone we don't need to keep a huge list of stones
    public override ValueTask<string> Solve_2() {
        var cur = stones.ToDictionary(x => x, x => 1L);
        var next = new Dictionary<long, long>();

        for (int i = 0; i < 75; i++)
        {
            foreach(var (num, count) in cur){
                var str = num.ToString();
                if(num == 0){
                    next[1] = next.GetValueOrDefault(1, 0) + count;
                }else if (str.Length % 2 == 0)
                {
                    var fh = Int64.Parse(str[..(str.Length/2)]);
                    var sh = Int64.Parse(str[(str.Length/2)..]);
                    next[fh] = next.GetValueOrDefault(fh, 0) + count;
                    next[sh] = next.GetValueOrDefault(sh, 0) + count;
                }else {
                    next[num * 2024] = next.GetValueOrDefault(num * 2024, 0) + count;
                }
            }
            cur = next;
            next = new Dictionary<long, long>();
        }

        return new ValueTask<string>($"{cur.Sum(x => x.Value)}");
    }

}