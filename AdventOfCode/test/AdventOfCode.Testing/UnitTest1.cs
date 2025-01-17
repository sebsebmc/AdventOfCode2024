using AdventOfCode;
using AoCHelper;
using NUnit.Framework;

namespace AdventOfCode.Testing;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(typeof(Day01), "11", "31")]
    [TestCase(typeof(Day02), "2", "4")]
    [TestCase(typeof(Day03), "161", "48")]
    [TestCase(typeof(Day04), "18", "9")]
    [TestCase(typeof(Day05), "143", "123")]
    [TestCase(typeof(Day06), "41", "Skipped")]
    [TestCase(typeof(Day07), "3749", "11387")]
    [TestCase(typeof(Day08), "14", "34")]
    [TestCase(typeof(Day09), "1928", "2858")]
    [TestCase(typeof(Day10), "36", "81")]   
    [TestCase(typeof(Day11), "55312", "65601038650482")]
    [TestCase(typeof(Day12), "1930", "1206")]
    public async Task Test(Type type, string sol1, string sol2)
    {
        // Can't use BaseDay since some of them aren't days, but you probably can
        if (Activator.CreateInstance(type) is BaseDay instance)
        {
            Assert.That(sol1, Is.EqualTo(await instance.Solve_1()));
            Assert.That(sol2, Is.EqualTo(await instance.Solve_2()));
        }
        else
        {
            Assert.Fail($"{type} is not a BaseProblem");
        }
    }
}