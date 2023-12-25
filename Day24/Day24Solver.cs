using Common;

namespace Day24;

internal class Day24Solver : Solver
{
    private List<Hailstone> _hailstones = new List<Hailstone>();

    public Day24Solver() : base(24, "Never Tell Me The Odds") { }

    protected override void ParseInput(string[] inputLines)
    {
        _hailstones = inputLines
            .Select(Hailstone.Parse)
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var minCoordinateValue = 200_000_000_000_000L;
        var maxCoordinateValue = 400_000_000_000_000L;

        var answer = 0;

        for (var i = 0; i < _hailstones.Count - 1; i++)
        {
            for (var j = i + 1; j < _hailstones.Count; j++)
            {
                var hailstone1 = _hailstones[i];
                var hailstone2 = _hailstones[j];
                
                var findIntersectionResult = FindXYIntersection(hailstone1, hailstone2);
                if (findIntersectionResult.Success &&
                    findIntersectionResult.Coordinate != null &&
                    findIntersectionResult.Coordinate.Value.X >= minCoordinateValue && findIntersectionResult.Coordinate.Value.X <= maxCoordinateValue &&
                    findIntersectionResult.Coordinate.Value.Y >= minCoordinateValue && findIntersectionResult.Coordinate.Value.Y <= maxCoordinateValue &&
                    HappensInTheFuture(hailstone1, findIntersectionResult.Coordinate.Value) &&
                    HappensInTheFuture(hailstone2, findIntersectionResult.Coordinate.Value))
                {
                    answer++;
                }                
            }
        }

        return answer.ToString(); // 20434
    }

    protected override string SolvePartTwo()
    {
        return "TODO";
    }

    private static (bool Success, Coordinate3D? Coordinate) FindXYIntersection(Hailstone hailstone1, Hailstone hailstone2)
    {
        // y = ax + c
        var a = 1.0M * hailstone1.Velocity.Y / hailstone1.Velocity.X;
        var c = hailstone1.Position.Y - a * hailstone1.Position.X;

        // y = bx + d
        var b = 1.0M * hailstone2.Velocity.Y / hailstone2.Velocity.X;
        var d = hailstone2.Position.Y - b * hailstone2.Position.X;

        if (a == b) // Parallel lines, same slope
        {
            return (false, null);
        }

        // y = ax + c
        // y = bx + d | (-) =>
        //   y - y = (a - b) * x + c - d =>
        //   0 = (a - b) * x + c - d =>
        //   x = (d - c) / (a - b)
        var x = (d - c) / (a - b);

        // y = bx + d, substitute x from above =>
        //   y = b * (d - c) / (a - b) + d
        var y = b * x + d;

        var coordinate = new Coordinate3D(Convert.ToInt64(x), Convert.ToInt64(y), 0);
        return (true, coordinate);
    }

    private static bool HappensInTheFuture(Hailstone hailstone, Coordinate3D coordinate)
    {
        return (coordinate.X - hailstone.Position.X) / hailstone.Velocity.X >= 0; // Position changed in the same direction as velocity
    }
}