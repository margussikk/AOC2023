using Common;

namespace Day15;

internal class Day15Solver : Solver
{
    private List<Operation> _operations = new List<Operation>();

    public Day15Solver() : base(15, "Lens Library") { }

    protected override void ParseInput(string[] inputLines)
    {
        _operations = inputLines[0]
            .Split(',')
            .Select(Operation.Parse)
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var answer = _operations.Sum(x => CalculateHash(x.ToString()));

        return answer.ToString(); // 517315
    }

    protected override string SolvePartTwo()
    {
        var boxes = Enumerable.Range(1, 256)
            .Select(x => new Box(x))
            .ToList();

        foreach (var operation in _operations)
        {
            var box = boxes[CalculateHash(operation.Label)];

            if (operation is RemoveLensOperation removeLensOperation)
            {
                box.RemoveLens(removeLensOperation.Label);
            }
            else if (operation is ReplaceLensOperation replaceLensOperation)
            {
                box.ReplaceLens(replaceLensOperation.Label, replaceLensOperation.FocalLength);
            }
        }

        var answer = boxes
            .Sum(x => x.CalculateFocalLength())
            .ToString();

        return answer; // 247763
    }

    private static int CalculateHash(string? input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        return input.Aggregate(0, (hash, current) => (hash + current) * 17 % 256);
    }
}