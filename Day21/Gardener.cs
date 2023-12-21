namespace Day21;

internal class Gardener
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int Steps { get; set; }

    public Gardener(int row, int column, int steps)
    {
        Row = row;
        Column = column;
        Steps = steps;
    }
}
