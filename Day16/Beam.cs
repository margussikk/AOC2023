namespace Day16;

internal class Beam
{
    public int Row { get; private set; }

    public int Column { get; private set; }

    public Direction Direction { get; private set; }

    public bool IsHorizontal => Direction is Direction.Right or Direction.Left;

    public bool IsVertical => Direction is Direction.Up or Direction.Down;

    public Beam(int row, int column, Direction direction)
    {
        Row = row;
        Column = column;
        Direction = direction;
    }

    public void ContinueSameDirection()
    {
        switch (Direction)
        {
            case Direction.Right:
                Column++;
                break;
            case Direction.Left:
                Column--;
                break;
            case Direction.Down:
                Row++;
                break;
            case Direction.Up:
                Row--;
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public void TurnLeft()
    {
        switch (Direction)
        {
            case Direction.Right:
                Row--;
                Direction = Direction.Up;
                break;
            case Direction.Left:
                Row++;
                Direction = Direction.Down;
                break;
            case Direction.Down:
                Column++;
                Direction = Direction.Right;
                break;
            case Direction.Up:
                Column--;
                Direction = Direction.Left;
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public void TurnRight()
    {
        switch (Direction)
        {
            case Direction.Left:
                Row--;
                Direction = Direction.Up;
                break;
            case Direction.Right:
                Row++;
                Direction = Direction.Down;
                break;
            case Direction.Down:
                Column--;
                Direction = Direction.Left;
                break;
            case Direction.Up:
                Column++;
                Direction = Direction.Right;
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public Beam Clone()
    {
        return new Beam(Row, Column, Direction);
    }

    public Beam[] SplitHorizontally()
    {
        return new Beam[2]
        {
            new Beam(Row, Column - 1, Direction.Left),
            new Beam(Row, Column + 1, Direction.Right),
        };
    }

    public Beam[] SplitVertically()
    {
        return new Beam[2]
        {
            new Beam(Row - 1, Column, Direction.Up),
            new Beam(Row + 1, Column, Direction.Down)
        };
    }
}
