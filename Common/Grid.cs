﻿namespace Common;

public class Grid<T>
{
    private readonly T[,] array;

    public int RowCount { get; }
    public int ColumnCount { get; }

    public Grid(int rows, int columns)
    {
        array = new T[rows, columns];

        RowCount = rows;
        ColumnCount = columns;
    }

    public T this[int row, int column]
    {
        get => array[row, column];
        set => array[row, column] = value;
    }

    public bool Contains(int row, int column)
    {
        return row >= 0 && row < RowCount &&
            column >= 0 && column < ColumnCount;
    }

    public IEnumerable<T> EnumerateAll()
    {
        for (var row = 0; row < RowCount; row++)
        {
            for (var column = 0; column < ColumnCount; column++)
            {
                yield return array[row, column];
            }
        }
    }

    public IEnumerable<T> EnumerateRow(int row)
    {
        for (var column = 0; column < ColumnCount; column++)
        {
            yield return array[row, column];
        }
    }

    public IEnumerable<T> EnumerateColumn(int column)
    {
        for (var row = 0; row < RowCount; row++)
        {
            
            yield return array[row, column];
        }
    }

    public IEnumerable<T> EnumerateSideNeighbors(int row, int column)
    {
        var deltas = new (int rowDelta, int columnDelta)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

        foreach (var (rowDelta, columnDelta) in deltas)
        {
            var newRow = row + rowDelta;
            var newColumn = column + columnDelta;

            if (Contains(newRow, newColumn))
            {
                yield return array[newRow, newColumn];
            }
        }
    }

    public Grid<T> Clone()
    {
        var grid = new Grid<T>(RowCount, ColumnCount);

        for (var row = 0; row < grid.RowCount; row++)
        {
            for (var column = 0; column < grid.ColumnCount; column++)
            {
                grid[row, column] = array[row, column];
            }
        }

        return grid;
    }

    public Grid<T> RotateCounterClockwise()
    {
        var grid = new Grid<T>(ColumnCount, RowCount);

        for (var row = 0; row < grid.RowCount; row++)
        {
            for (var column = 0; column < grid.ColumnCount; column++)
            {
                grid[row, column] = array[column, grid.RowCount - 1 - row];
            }
        }

        return grid;
    }

    public Grid<T> RotateClockwise()
    {
        var grid = new Grid<T>(ColumnCount, RowCount);

        for (var row = 0; row < grid.RowCount; row++)
        {
            for (var column = 0; column < grid.ColumnCount; column++)
            {
                grid[row, column] = array[grid.ColumnCount - 1 - column, row];
            }
        }

        return grid;
    }
}
