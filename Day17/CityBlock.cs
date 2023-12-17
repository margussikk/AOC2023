namespace Day17;

internal class CityBlock
{
    public int Row { get; }

    public int Column { get; }

    public int HeatLoss { get; }

    public CityBlock(int row, int column, int heatLoss)
    {
        Row = row;
        Column = column;
        HeatLoss = heatLoss;
    }
}
