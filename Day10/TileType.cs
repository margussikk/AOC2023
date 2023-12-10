namespace Day10;

[Flags]
internal enum TileType
{
#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
    Ground = 0,
#pragma warning restore S2346 // Flags enumerations zero-value members should be named "None"
    North = 1 << 0,
    East = 1 << 1,
    South = 1 << 2,
    West = 1 << 3,
    Start = 1 << 4,
}
