namespace Common;

public class Vector2D
{
    public long X { get; }

    public long Y { get; }

    public long DX { get; }

    public long DY { get; }

    public Vector2D(long x, long y, long dX, long dY)
    {
        X = x;
        Y = y;
        DX = dX;
        DY = dY;
    }
}
