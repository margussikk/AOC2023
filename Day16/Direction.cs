namespace Day16;

[Flags]
internal enum Direction
{
    Up = 1 << 0,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3
}
