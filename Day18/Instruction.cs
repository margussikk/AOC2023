using System.Globalization;

namespace Day18;

internal class Instruction
{
    public Direction Direction { get; private set; }

    public long Distance { get; private set; }

    public static Instruction Part1Parse(string input)
    {
        var splits = input.Split(' ');

        var instruction = new Instruction()
        {
            Direction = splits[0][0] switch
            {
                'R' => Direction.Right,
                'D' => Direction.Down,
                'L' => Direction.Left,
                'U' => Direction.Up,
                _ => throw new InvalidOperationException(),
            },
            Distance = long.Parse(splits[1])
        };

        return instruction;
    }

    public static Instruction Part2Parse(string input)
    {
        var splits = input.Split(' ');

        var value = splits[2][2..^1];

        var instruction = new Instruction()
        {
            Direction = value[5] switch
            {
                '0' => Direction.Right,
                '1' => Direction.Down,
                '2' => Direction.Left,
                '3' => Direction.Up,
                _ => throw new InvalidOperationException(),
            },
            Distance = long.Parse(value.Substring(0, 5), NumberStyles.HexNumber)
        };

        return instruction;
    }
}
