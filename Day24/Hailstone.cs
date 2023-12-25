using Common;

namespace Day24;

internal class Hailstone
{
    public Coordinate3D Position { get; set; }

    public Coordinate3D Velocity { get; set; }

    public static Hailstone Parse(string input)
    {
        var splits = input.Split('@');

        var positionCoordinates = splits[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();

        var velocityCoordinates = splits[1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();

        var hailstone = new Hailstone
        {
            Position = new Coordinate3D(positionCoordinates[0], positionCoordinates[1], positionCoordinates[2]),
            Velocity = new Coordinate3D(velocityCoordinates[0], velocityCoordinates[1], velocityCoordinates[2]),
        };

        return hailstone;
    }
}
