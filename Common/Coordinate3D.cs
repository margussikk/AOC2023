using System.Diagnostics.CodeAnalysis;

namespace Common;

public readonly struct Coordinate3D
{
    public long X { get; }

    public long Y { get; }

    public long Z { get; }

    public Coordinate3D(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate3D otherCoordinate && X == otherCoordinate.X && Y == otherCoordinate.Y && Z == otherCoordinate.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public IEnumerable<Coordinate3D> EnumerateSideCoordinates()
    {
        var vectors = new Coordinate3D[]
        {
            new Coordinate3D(1, 0, 0),
            new Coordinate3D(-1, 0, 0),

            new Coordinate3D(0, 1, 0),
            new Coordinate3D(0, -1, 0),

            new Coordinate3D(0, 0, 1),
            new Coordinate3D(0, 0, -1),
        };

        foreach (var vector in vectors)
        {
            yield return this + vector;
        }
    }

    public static bool operator ==(Coordinate3D left, Coordinate3D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate3D left, Coordinate3D right)
    {
        return !(left == right);
    }

    public static Coordinate3D operator +(Coordinate3D left, Coordinate3D right)
    {
        return new Coordinate3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }
}
