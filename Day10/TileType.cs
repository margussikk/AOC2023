namespace Day10;

[Flags]
internal enum TileType
{
    Ground = 0,
    North = 1 << 0,
    East = 1 << 1,
    South = 1 << 2,
    West = 1 << 3,
    Start = 1 << 4,
}
