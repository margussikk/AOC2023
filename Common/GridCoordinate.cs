namespace Common;

public struct GridCoordinate
{
    public readonly int Row { get; }

    public readonly int Column { get; }

    public GridCoordinate(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public IEnumerable<GridCoordinate> Sides()
    {
        yield return new GridCoordinate(Row - 1, Column); // Top
        yield return new GridCoordinate(Row, Column + 1); // Right
        yield return new GridCoordinate(Row + 1, Column); // Bottom
        yield return new GridCoordinate(Row, Column - 1); // Left
    }

    public static bool operator ==(GridCoordinate gridCoordinate1, GridCoordinate gridCoordinate2)
    {
        return gridCoordinate1.Equals(gridCoordinate2);
    }

    public static bool operator !=(GridCoordinate gridCoordinate1, GridCoordinate gridCoordinate2)
    {
        return !gridCoordinate1.Equals(gridCoordinate2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }

    public override bool Equals(object? obj)
    {
        return (obj is GridCoordinate gridCoordinate)
            && Row == gridCoordinate.Row
            && Column == gridCoordinate.Column;
    }
}
