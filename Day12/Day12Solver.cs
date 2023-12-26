using Common;

namespace Day12;

internal partial class Day12Solver : Solver
{
    private List<Record> _records = new List<Record>();

    public Day12Solver() : base(12, "Hot Springs") { }

    protected override void ParseInput(string[] inputLines)
    {
        _records = inputLines
            .Select(Record.Parse)
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var cache = new Dictionary<string, long>();

        var answer = _records
            .Select(x => x.CountArrangements(cache))
            .Sum()
            .ToString();

        return answer; // 7032
    }    

    protected override string SolvePartTwo()
    {
        var cache = new Dictionary<string, long>();

        var answer = _records
            .Select(x => x.Unfolded())
            .Select(x => x.CountArrangements(cache))
            .Sum()
            .ToString();

        return answer.ToString(); // 1493340882140
    }
}