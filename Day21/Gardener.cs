using Common;

namespace Day21;

internal class Gardener
{
    public GridCoordinate Coordinate { get; set; }

    public int Steps { get; set; }

    public Gardener(GridCoordinate coordinate, int steps)
    {
        Coordinate = coordinate;
        Steps = steps;
    }
}
