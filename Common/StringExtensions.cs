namespace Common;

public static class StringExtensions
{
    public static Grid<T> MapToGrid<T>(this string[] lines, Func<int, int, char, T> func)
    {
        var grid = new Grid<T>(lines.Length, lines[0].Length);

        for (var row = 0; row < grid.RowCount; row++)
        {
            for (var column = 0; column < grid.ColumnCount; column++)
            {
                grid[row, column] = func(row, column, lines[row][column]);
            }
        }

        return grid;
    }
}
