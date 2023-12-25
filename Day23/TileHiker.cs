namespace Day23;

internal class TileHiker
{
    public Tile StartTile { get; }

    public Tile CurrentTile { get; }

    public Direction Direction { get; }

    public int Distance { get; set; }

    public TileHiker (Tile startTile, Tile currentTile, Direction direction, int distance)
    {
        StartTile = startTile;
        CurrentTile = currentTile;
        Direction = direction;
        Distance = distance;
    }
}
