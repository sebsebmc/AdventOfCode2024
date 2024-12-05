namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string _input;
    private Dictionary<int, List<int>> por;
    private List<List<int>> updates = [];

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath);
        por = new Dictionary<int, List<int>>();
        var parts = _input.Split("\n\n");
        foreach (var line in parts[0].Split("\n"))
        {
            var nums = line.Split("|");
            int key = Int32.Parse(nums[0]);
            var after = por.GetValueOrDefault(key, new List<int>());
            after.Add(Int32.Parse(nums[1]));
            por[key] = after;
        }
        foreach(var line in parts[1].Split("\n")){
            updates.Add(line.Split(",").Select((v, _) => Int32.Parse(v)).ToList());
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        foreach (var update in updates)
        {
            for (int i = 0; i < update.Count-1; i++)
            {
                if(!por.ContainsKey(update[i]) || por[update[i]].IndexOf(update[i+1]) == -1) {
                    goto Bad;
                } 
            }
            sum += update[update.Count/2];
            Bad:
                continue;
        }
        return new ValueTask<string>(sum.ToString());
    }

    private List<int> Order(List<int> update){
        var remaining = update.ToList();
        var ordered = new List<int>();
        while (remaining.Count > 0)
        {
            for(var i = 0; i < remaining.Count; i++){
                if (!por.ContainsKey(remaining[i]))
                {
                    if(remaining.Count == 1){
                        ordered.Add(remaining[i]);
                        return ordered;
                    }
                    continue;
                }
                if(IsFirst(remaining, i)){
                    ordered.Add(remaining[i]);
                    remaining.Remove(remaining[i]);
                }
            }
        }
        return ordered;
    }

    private bool IsFirst(List<int> update, int i)
    {
        for (int j = 0; j < update.Count; j++)
        {
            if (update[i] == update[j])
            {
                continue; //skip self
            }
            if (por[update[j]].Contains(update[i]))
            {
                // this page is not next
                return false;
            }
        }
        return true;
    }

    public override ValueTask<string> Solve_2()  
    {
        var sum = 0;
        foreach (var update in updates)
        {
            bool wasBad = false;
            for (int i = 0; i < update.Count-1; i++)
            {
                if(!por.ContainsKey(update[i]) || por[update[i]].IndexOf(update[i+1]) == -1) {
                    wasBad = true;
                } 
            }
            if(wasBad){
                var oUpdate = Order(update);
                sum += oUpdate[oUpdate.Count/2];
            }
        }
        return new ValueTask<string>(sum.ToString());
    }

}