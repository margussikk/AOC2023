namespace Common;

public struct GridCoordinate
{
    public int Row { get; }
    public int Column { get; }

    public GridCoordinate(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
