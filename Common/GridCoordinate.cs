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
