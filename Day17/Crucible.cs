
namespace Day17;

internal class Crucible
{
    public CityBlock CityBlock { get; private set; }

    public Crucible? PreviousCrucible { get; private set; }

    public Direction Direction { get; private set; }

    public int Steps { get; private set; }

    public int HeatLoss { get; private set; }

    public (int, int, Direction, int) State => (CityBlock.Row, CityBlock.Column, Direction, Steps);

    public Crucible(CityBlock cityBlock, Crucible? previousCrucible, Direction direction, int steps, int heatLoss)
    {
        CityBlock = cityBlock;
        PreviousCrucible = previousCrucible;
        Direction = direction;
        Steps = steps;
        HeatLoss = heatLoss;
    }
}
