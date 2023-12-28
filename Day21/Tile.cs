namespace Day21;

internal class Tile
{
    public int Row { get; }
    public int Column { get; }
    public TileType TileType { get; }

    public Tile(int row, int column, TileType tileType)
    {
        Row = row;
        Column = column;
        TileType = tileType;
    }
}
