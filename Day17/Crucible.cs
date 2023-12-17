
namespace Day17;

internal class Crucible
{
    public CityBlock CityBlock { get; private set; }

    public Crucible? PreviousCrucible { get; private set; }

    public Direction Direction { get; private set; }

    public int Steps { get; private set; }

    public int HeatLoss { get; private set; }

    public Crucible(CityBlock cityBlock, Crucible? previousCrucible, Direction direction, int steps, int heatLoss)
    {
        CityBlock = cityBlock;
        PreviousCrucible = previousCrucible;
        Direction = direction;
        Steps = steps;
        HeatLoss = heatLoss;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(CityBlock.Row.GetHashCode());
        hashCode.Add(CityBlock.Column.GetHashCode());
        hashCode.Add(Direction.GetHashCode());
        hashCode.Add(Steps.GetHashCode());

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Crucible crucible &&
               EqualityComparer<CityBlock>.Default.Equals(CityBlock, crucible.CityBlock) &&
               EqualityComparer<Crucible?>.Default.Equals(PreviousCrucible, crucible.PreviousCrucible) &&
               Direction == crucible.Direction &&
               Steps == crucible.Steps &&
               HeatLoss == crucible.HeatLoss;
    }
}
