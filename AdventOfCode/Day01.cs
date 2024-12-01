﻿namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);

    }

    private (List<int>, List<int>) getLists() {
        var list1 = new List<int>();
        var list2 =  new List<int>();

        foreach(var line in _input.Split("\n")[..^1]){
            var words = line.Split("   ");
            list1.Add(Int32.Parse(words[0]));
            list2.Add(Int32.Parse(words[1]));
        }
        return (list1, list2);
    }

    public override ValueTask<string> Solve_1()  {
        var (list1, list2) = getLists();

        list1.Sort();
        list2.Sort();

        var sum = 0;
        for (int i = 0; i < list1.Count; i++)
        {
            sum += Math.Abs(list1[i] - list2[i]);
        }
        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2() {
        var (list1, list2) = getLists();

        var sum = 0;
        foreach(var num in list1.Distinct()){
            sum += num * list2.FindAll(x => x == num).Count;
        }

        return new ValueTask<string>(sum.ToString());
    }
}
