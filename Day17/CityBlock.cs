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

    public Direction DirectionFrom(CityBlock previousCityBlock)
    {
        if (Row == previousCityBlock.Row && Column == previousCityBlock.Column)
        {
            return Direction.None;
        }
        else if (Row == previousCityBlock.Row)
        {
            return Column > previousCityBlock.Column ? Direction.Right : Direction.Left;
        }
        else
        {
            return Row > previousCityBlock.Row ? Direction.Down : Direction.Up;
        }
    }
}
