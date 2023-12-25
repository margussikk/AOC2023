using Common;

namespace Day23;

internal class Tile
{
    public int Row { get; }
    public int Column { get; }
    public TileType TileType { get; }

    public GridCoordinate Coordinate => new GridCoordinate(Row, Column);

    public Tile(int row, int column, TileType tileType)
    {
        Row = row;
        Column = column;
        TileType = tileType;
    }

    public Direction DirectionFrom(Tile previousTile)
    {
        if (Row == previousTile.Row)
        {
            return Column > previousTile.Column ? Direction.Right : Direction.Left;
        }
        else
        {
            return Row > previousTile.Row ? Direction.Down : Direction.Up;
        }
    }
}
