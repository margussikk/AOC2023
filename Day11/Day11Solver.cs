using Common;

namespace Day11;

internal partial class Day11Solver : Solver
{
    private List<Galaxy> _galaxies = new List<Galaxy>();
    private List<int> _expandingRows = new List<int>();
    private List<int> _expandingColumns = new List<int>();

    public Day11Solver() : base(11, "Cosmic Expansion") { }

    protected override void ParseInput(string[] inputLines)
    {
        _galaxies = inputLines
            .SelectMany((line, row) => line
                .Select((letter, column) => (letter, column))
                .Where(x => x.letter == '#')
                .Select(x => new Galaxy(row, x.column))
                .ToList())
            .ToList();

        _expandingRows = Enumerable.Range(0, inputLines.Length)
            .Where(r => _galaxies.TrueForAll(g => g.Row != r))
            .ToList();

        _expandingColumns = Enumerable.Range(0, inputLines[0].Length)
            .Where(c => _galaxies.TrueForAll(g => g.Column != c))
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var answer = GetDistance(2L);

        return answer.ToString(); // 10077850
    }    

    protected override string SolvePartTwo()
    {
        var answer = GetDistance(1_000_000L);

        return answer.ToString(); // 504715068438
    }

    private long GetDistance(long expansionFactor)
    {
        long sumOfLengths = 0L;

        foreach (var (galaxy1, galaxy1Index) in _galaxies.Select((item, index) => (item, index)).Take(_galaxies.Count - 1))
        {
            foreach(var galaxy2 in _galaxies.Skip(galaxy1Index + 1))
            {
                var minRow = Math.Min(galaxy1.Row, galaxy2.Row);
                var maxRow = Math.Max(galaxy1.Row, galaxy2.Row);
                var minColumn = Math.Min(galaxy1.Column, galaxy2.Column);
                var maxColumn = Math.Max(galaxy1.Column, galaxy2.Column);

                long directDistance = (maxRow - minRow) + (maxColumn - minColumn);

                var emptyRowsBetween = _expandingRows.Count(x => x > minRow && x < maxRow);
                var emptyColumnsBetween = _expandingColumns.Count(x => x > minColumn && x < maxColumn);
                var expandedDistance = (emptyRowsBetween + emptyColumnsBetween) * (expansionFactor - 1);

                sumOfLengths += directDistance + expandedDistance;
            }
        }

        return sumOfLengths;
    }
}