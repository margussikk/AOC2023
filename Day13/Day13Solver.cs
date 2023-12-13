using Common;

namespace Day13;

internal partial class Day13Solver : Solver
{
    private readonly List<Grid<Terrain>> _grids = new List<Grid<Terrain>>();

    public Day13Solver() : base(13, "Point of Incidence") { }

    protected override void ParseInput(string[] inputLines)
    {
        var lists = new List<List<string>>
        {
            new List<string>()
        };

        for (var i = 0; i < inputLines.Length; i++)
        {
            if (string.IsNullOrEmpty(inputLines[i]))
            {
                // Add new list
                lists.Add(new List<string>());
            }
            else
            {
                lists[^1].Add(inputLines[i]);
            }
        }

        foreach (var list in lists)
        {
            var grid = new Grid<Terrain>(list.Count, list[0].Length);

            for (var row = 0; row < list.Count; row++)
            {
                for (var column = 0; column < list[row].Length; column++)
                {
                    grid[row, column] = list[row][column] switch
                    {
                        '.' => Terrain.Ash,
                        '#' => Terrain.Rock,
                        _ => throw new NotImplementedException()
                    };
                }
            }

            _grids.Add(grid);
        }
    }

    protected override string SolvePartOne()
    {
        var answer = _grids
            .Select(grid => CountReflectedRows(grid, 0) * 100 + CountReflectedColumns(grid, 0))
            .Sum()
            .ToString();

        return answer; // 33047
    }    

    protected override string SolvePartTwo()
    {
        var answer = _grids
            .Select(grid => CountReflectedRows(grid, 1) * 100 + CountReflectedColumns(grid, 1))
            .Sum()
            .ToString();

        return answer; // 28806
    }


    private static int CountReflectedColumns(Grid<Terrain> grid, int smudgeCount)
    {
        var count = 0;

        for (var column = 0; column < grid.ColumnCount - 1; column++)
        {
            var column1Terrains = grid.EnumerateColumn(column).ToList();
            var column2Terrains = grid.EnumerateColumn(column + 1).ToList();

            var differences = CountDifferences(column1Terrains, column2Terrains);
            if (differences <= smudgeCount) // Possible reflection
            {
                var maxReflectionLength = Math.Min(column, grid.ColumnCount - 1 - column - 1); // Length towards closest edge
                for(var i = 1; i <= maxReflectionLength; i++)
                {
                    column1Terrains = grid.EnumerateColumn(column - i).ToList();
                    column2Terrains = grid.EnumerateColumn(column + i + 1).ToList();

                    differences += CountDifferences(column1Terrains, column2Terrains);
                    if (differences > smudgeCount)
                    {
                        break;
                    }
                }
            }

            if (differences == smudgeCount)
            {
                count += column + 1;
            }
        }

        return count;
    }

    private static int CountReflectedRows(Grid<Terrain> grid, int smudgeCount)
    {
        var count = 0;

        for (var row = 0; row < grid.RowCount - 1; row++)
        {
            var row1Terrains = grid.EnumerateRow(row).ToList();
            var row2Terrains = grid.EnumerateRow(row + 1).ToList();

            var differences = CountDifferences(row1Terrains, row2Terrains);
            if (differences <= smudgeCount) // Possible reflection
            {
                var maxReflectionLength = Math.Min(row, grid.RowCount - 1 - row - 1); // Length towards closest edge
                for (var i = 1; i <= maxReflectionLength; i++)
                {
                    row1Terrains = grid.EnumerateRow(row - i).ToList();
                    row2Terrains = grid.EnumerateRow(row + 1 + i).ToList();

                    differences += CountDifferences(row1Terrains, row2Terrains);
                    if (differences > smudgeCount)
                    {
                        break;
                    }
                }
            }

            if (differences == smudgeCount)
            {
                count += row + 1;
            }
        }

        return count;
    }

    private static int CountDifferences(List<Terrain> terrains1, List<Terrain> terrains2)
    {
        var counter = Enumerable
            .Range(0, terrains1.Count)
            .Count(i => terrains1[i] != terrains2[i]);
        
        return counter;
    }
}