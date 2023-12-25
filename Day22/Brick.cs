using Common;

namespace Day22;

internal class Brick
{
    public int Id { get; private set; } // Good for debugging
    public Coordinate3D Start { get; private set; }
    public Coordinate3D End { get; private set; }

    public List<Brick> Supports { get; private set; } = new List<Brick>();

    public List<Brick> SupportedBy { get; private set; } = new List<Brick>();

    public Brick CleanClone()
    {
        var clone = new Brick()
        {
            Id = Id,
            Start = Start,
            End = End,
        };

        return clone;
    }

    public void AddSupports(Brick brick)
    {
        if (!Supports.Contains(brick))
        {
            Supports.Add(brick);
        }
    }

    public void AddSupportedBy(Brick brick)
    {
        if (!SupportedBy.Contains(brick))
        {
            SupportedBy.Add(brick);
        }
    }

    public void DropToZ(long z)
    {
        if (Start.Z != z)
        {
            // NB! Set End before Start
            End = new Coordinate3D(End.X, End.Y, End.Z - Start.Z + z);
            Start = new Coordinate3D(Start.X, Start.Y, z);
        }
    }

    public static Brick Parse(int id, string input)
    {
        var ends = input.Split('~')
                        .Select(ParseEnd)
                        .ToArray();

        if (ends[0].X != ends[1].X)
        {
            ends = ends.OrderBy(x => x.X).ToArray();
        }
        else if (ends[0].Y != ends[1].Y)
        {
            ends = ends.OrderBy(x => x.Y).ToArray();
        }
        else
        {
            ends = ends.OrderBy(x => x.Z).ToArray();
        }

        return new Brick
        {
            Id = id,
            Start = ends[0],
            End = ends[1],
        };
    }

    public static Brick CreateGroundBrick(long maxX, long maxY)
    {
        return new Brick
        {
            Id = 0,
            Start = new Coordinate3D(0, 0, 0),
            End = new Coordinate3D(maxX, maxY, 0),
        };
    }

    private static Coordinate3D ParseEnd(string input)
    {
        var values = input.Split(',')
                          .Select(int.Parse)
                          .ToList();        
        
        return new Coordinate3D(values[0], values[1], values[2]);
    }
}
