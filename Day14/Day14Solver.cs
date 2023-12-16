using Common;

namespace Day14;

internal class Day14Solver : Solver
{
    private Grid<Item> _platform = new Grid<Item>(0, 0);

    public Day14Solver() : base(14, "Parabolic Reflector Dish") { }

    protected override void ParseInput(string[] inputLines)
    {
        var platform = new Grid<Item>(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < platform.RowCount; row++)
        {
            for (var column = 0;  column < platform.ColumnCount; column++)
            {
                platform[row, column] = inputLines[row][column] switch
                {
                    '.' => Item.EmptySpace,
                    'O' => Item.RoundedRock,
                    '#' => Item.CubeShapedRock,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        _platform = platform;
    }

    protected override string SolvePartOne()
    {
        var platform = _platform.Clone();

        TiltToNorth(platform);

        var answer = CalculateTotalLoad(platform);

        return answer.ToString(); // 109833
    }

    protected override string SolvePartTwo()
    {
        var foundCycle = false;
        var platformCycles = new Dictionary<int, int>();

        var platform = _platform.Clone();

        for (var cycle = 0; cycle < 1_000_000_000; cycle++)
        {
            if (!foundCycle)
            {
                var hashCode = platform.GetHashCode();
                if (platformCycles.TryGetValue(hashCode, out var platformCycle))
                {
                    foundCycle = true;

                    var cycleLength = cycle - platformCycle;

                    var fastForwardCyclesCount = (1_000_000_000 - cycle) / cycleLength;

                    cycle += fastForwardCyclesCount * cycleLength;
                }
                else
                {
                    platformCycles[hashCode] = cycle;
                }
            }

            platform = SpinCycle(platform);
        }

        var answer = CalculateTotalLoad(platform);

        return answer.ToString(); // 99875
    }

    private static Grid<Item> SpinCycle(Grid<Item> platform)
    {
        // Instead of tilting to west, south and east we rotate the grid and always tilt to the north

        // North
        TiltToNorth(platform);

        // West
        platform = platform.RotateClockwise();
        TiltToNorth(platform);

        // South
        platform = platform.RotateClockwise();
        TiltToNorth(platform);

        // East
        platform = platform.RotateClockwise();
        TiltToNorth(platform);

        // Rotate back to original direction
        platform = platform.RotateClockwise();

        return platform;
    }

    private static void TiltToNorth(Grid<Item> platform)
    {
        for (var row = 0; row < platform.RowCount; row++)
        {
            for (var column = 0; column < platform.ColumnCount; column++)
            {
                if (platform[row, column] == Item.RoundedRock)
                {
                    var rolledToRow = row;
                    while (rolledToRow > 0 && platform[rolledToRow - 1, column] == Item.EmptySpace)
                    {
                        rolledToRow--;
                    }

                    if (rolledToRow != row)
                    {
                        platform[rolledToRow, column] = Item.RoundedRock;
                        platform[row, column] = Item.EmptySpace;
                    }
                }
            }
        }
    }

    private static int CalculateTotalLoad(Grid<Item> platform)
    {
        return Enumerable.Range(0, platform.RowCount)
            .Sum(row => (platform.RowCount - row) *
                platform.EnumerateRow(row)
                        .Count(x => x == Item.RoundedRock));
    }

    private static void Print(Grid<Item> platform)
    {
        for (var row = 0; row < platform.RowCount; row++)
        {
            for (var column = 0; column < platform.ColumnCount; column++)
            {
                var letter = platform[row, column] switch
                {
                    Item.EmptySpace => '.',
                    Item.RoundedRock => 'O',
                    Item.CubeShapedRock => '#',
                    _ => throw new NotImplementedException()
                };

                Console.Write(letter);
            }
            Console.WriteLine();
        }
    }
}