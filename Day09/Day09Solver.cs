using Common;

namespace Day09;

internal partial class Day09Solver : Solver
{
    private List<History> _histories = new List<History>();

    public Day09Solver() : base(9, "Mirage Maintenance") { }

    protected override void ParseInput(string[] inputLines)
    {
        _histories = inputLines
            .Select(History.Parse)
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var answer = _histories
            .Select(history => Enumerable
                .Range(0, history.Sequences.Count - 1)
                .Reverse()
                .Aggregate(0L, (extrapolatedValue, sequenceIndex) => history.Sequences[sequenceIndex][^1] + extrapolatedValue))
            .Sum()
            .ToString();

        return answer; // 1681758908
    }

    protected override string SolvePartTwo()
    {
        var answer = _histories
            .Select(history => Enumerable
                .Range(0, history.Sequences.Count - 1)
                .Reverse()
                .Aggregate(0L, (extrapolatedValue, sequenceIndex) => history.Sequences[sequenceIndex][0] - extrapolatedValue))
            .Sum()
            .ToString();

        return answer; // 803
    }
}