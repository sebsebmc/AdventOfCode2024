namespace AdventOfCode;

public class Day09 : BaseDay {
    private readonly string _input;
    List<int> map = [];
    int diskSize = 0;

    public Day09() {
        _input = File.ReadAllText(InputFilePath);
        diskSize = 0;
        foreach (char c in _input) {
            var temp = Int32.Parse(c.ToString());
            map.Add(temp);
            diskSize += temp;
        }
    }

    public override ValueTask<string> Solve_1() {
        var minId = 0;
        var frontIdx = 0;
        var freeIdx = 1;
        var backIdx = map.Count - 1;
        var maxId = map.Count / 2;
        var endBlocksRem = map[^1];
        var freeBlockRem = map[freeIdx];
        var diskPos = 0;
        var endPos = diskSize - 1;

        long sum = 0;

        while (endPos > diskPos + endBlocksRem) {
            for (int i = 0; i < map[frontIdx]; i++) {
                sum += diskPos * minId;
                diskPos++;
            }

            while (freeBlockRem > 0 && endPos > diskPos + endBlocksRem) {
                freeBlockRem--;
                endBlocksRem--;
                sum += diskPos * maxId;
                diskPos++;
                endPos--;

                if (endBlocksRem == 0) {
                    maxId--;
                    backIdx -= 2;
                    endPos -= map[backIdx + 1];

                    endBlocksRem = map[backIdx];
                }
            }
            frontIdx += 2;
            freeIdx += 2;
            freeBlockRem = map[freeIdx];
            minId += 1;
        }
        while (endBlocksRem > 0) {
            sum += diskPos * maxId;
            diskPos++;
            endBlocksRem--;
        }

        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2() {
        var backIdx = map.Count - 1;
        var maxId = map.Count / 2;
        var diskPos = 0;
        // for speed (of about 8%)
        var firstFreeIdx = 1;

        long sum = 0;
        var map2 = new List<(int id, int size)>();

        for (int i = 0; i < map.Count; i++) {
            var id = i % 2 == 0 ? i / 2 : -1;
            map2.Add((id, map[i]));
        }
        map2.Add((-1, 1));

        var oldLoc = map2.Count - 1;
        while (maxId > 0) {
            for (int i = oldLoc; i >= 0; i--) {
                if (map2[i].id == maxId) {
                    oldLoc = i;
                    break;
                }
            }
            if (oldLoc < firstFreeIdx) {
                break;
            }
            for (int i = firstFreeIdx; i < map2.Count; i++) {
                var (id, size) = map2[i];
                if (id == map2[oldLoc].id) {
                    break;
                }
                if (id != -1 || size == 0) {
                    continue;
                }
                if (size == map2[oldLoc].size) {
                    map2[i] = map2[oldLoc];
                    map2[oldLoc] = (-1, map2[oldLoc].size);
                    if (i == firstFreeIdx) {
                        while (map2[firstFreeIdx].id != -1) {
                            firstFreeIdx++;
                        }
                    }
                    break;
                }
                if (size > map2[oldLoc].size) {
                    map2[i] = map2[oldLoc];
                    map2[oldLoc] = (-1, map2[oldLoc].size);
                    map2.Insert(i + 1, ((-1, size - map2[oldLoc].size)));
                    if (i == firstFreeIdx) {
                        firstFreeIdx++;
                    }
                    break;
                }

            }
            maxId--;
        }
        foreach (var (id, size) in map2) {
            for (int i = 0; i < size; i++) {
                if (id != -1) {
                    sum += id * diskPos;
                } else {
                }
                diskPos++;
            }
        }

        return new ValueTask<string>($"{sum}");
    }

}