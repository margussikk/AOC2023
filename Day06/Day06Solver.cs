using Common;

namespace Day06;

internal class Day06Solver : Solver
{
    private readonly List<Race> _races = new List<Race>();
   
    public Day06Solver() : base(6, "Wait For It") { }

    protected override void ParseInput(string[] inputLines)
    {
        var times = inputLines[0]
            .Substring(inputLines[0].IndexOf(':') + 1)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        var distances = inputLines[1]
            .Substring(inputLines[1].IndexOf(':') + 1)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        for (var i = 0; i < times.Count; i++)
        {
            var race = new Race()
            {
                Time = times[i],
                Distance = distances[i]
            };

            _races.Add(race);
        }
    }

    protected override string SolvePartOne()
    {
        var answer = _races
            .Select(race => race.GetNumberOfWaysToBeatTheRecord())
            .Aggregate(1L, (acc, current) => acc * current)
            .ToString();

        return answer; // 588588
    }

    protected override string SolvePartTwo()
    {
        var race = new Race()
        {
            Time = long.Parse(string.Concat(_races.Select(x => x.Time))),
            Distance = long.Parse(string.Concat(_races.Select(x => x.Distance))),
        };

        var answer = race.GetNumberOfWaysToBeatTheRecord()
            .ToString();

        return answer; // 34655848
    }
}
