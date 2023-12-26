namespace Day12;

internal class Record
{
    public List<Spring> Springs { get; }
    public List<int> Groups { get; }

    public Record(List<Spring> springs, List<int> groups)
    {
        Springs = springs;
        Groups = groups;
    }

    public static Record Parse(string input)
    {
        var splits = input.Split(' ');

        var springs = splits[0].Select(ParseSpringCondition)
                               .ToList();

        var groups = splits[1].Split(',')
                              .Select(int.Parse)
                              .ToList();

        return new Record(springs, groups);
    }

    public Record Unfolded()
    {
        var springs = new List<Spring>(Springs);
        var groups = new List<int>(Groups);

        for (var i = 0; i < 4; i++)
        {
            springs.Add(Spring.Unknown);
            springs.AddRange(Springs);

            groups.AddRange(Groups);
        }

        return new Record(springs, groups);
    }

    public long CountArrangements(Dictionary<string, long> cache)
    {      
        if (Groups.Count == 0)
        {
            if (Springs.TrueForAll(x => x is Spring.Operational or Spring.Unknown)) // Assume that all the leftover springs are operational
            {
                return 1;
            }

            return 0;
        }

        if (Springs.Count < Groups.Sum() + Groups.Count - 1)
        {
            return 0; // Not enough springs for all the groups
        }

        var cacheKey = ToString();
        if (cache.TryGetValue(cacheKey, out long total))
        {
            return total;
        }

        // Treat first spring as operational
        if (Springs[0] is Spring.Operational or Spring.Unknown)
        {
            var newRecord = new Record(Springs[1..], Groups);
            total += newRecord.CountArrangements(cache);
        }

        // Treat first {groupSize} springs as damaged
        var groupSize = Groups[0];
        if (Springs[..groupSize].TrueForAll(x => x is Spring.Damaged or Spring.Unknown))
        {
            if (Springs.Count == groupSize)
            {
                var newRecord = new Record(Springs[groupSize..], Groups[1..]);
                total += newRecord.CountArrangements(cache);
            }
            else if (Springs[groupSize] is Spring.Operational or Spring.Unknown)
            {
                var newRecord = new Record(Springs[(groupSize + 1)..], Groups[1..]);
                total += newRecord.CountArrangements(cache);
            }
        }

        cache[cacheKey] = total;

        return total;
    }

    public override string ToString()
    {
        var text = Springs.Select(spring =>
        {
            return spring switch
            {
                Spring.Operational => '.',
                Spring.Damaged => '#',
                Spring.Unknown => '?',
                _ => throw new InvalidOperationException()
            };
        }).ToArray();

        return string.Join(string.Empty, text) + " " + string.Join(",", Groups);
    }

    private static Spring ParseSpringCondition(char letter)
    {
        return letter switch
        {
            '.' => Spring.Operational,
            '#' => Spring.Damaged,
            '?' => Spring.Unknown,
            _ => throw new InvalidOperationException()
        };
    }
}
