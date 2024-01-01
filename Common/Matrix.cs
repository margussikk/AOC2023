using System.Numerics;

namespace Common;

public class Matrix<T> where T : INumber<T>
{
    private readonly T[,] _array;

    public int RowCount { get; }
    public int ColumnCount { get; }

    public int LastRowIndex => RowCount - 1;

    public int LastColumnIndex => ColumnCount - 1;

    public Matrix(int rows, int columns)
    {
        _array = new T[rows, columns];

        RowCount = rows;
        ColumnCount = columns;
    }

    public T this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }

    public void SetRow(int row, T[] elements)
    {
        if (elements.Length != ColumnCount)
        {
            throw new InvalidOperationException("Element count and matrix column mismatch");
        }

        for (var i =  0; i < ColumnCount; i++)
        {
            _array[row, i] = elements[i];
        }
    }
}
