using Common;

namespace Day10;

internal class Tile
{
    public int Row { get; private set; }

    public int Column { get; private set; }

    public TileType TileType { get; private set; } = TileType.Ground;

    public bool IsAlongTheLoop { get; set; }

    public void DetermineStartTileType(Grid<Tile> tiles)
    {
        if (tiles.Contains(Row - 1, Column) && tiles[Row - 1, Column].TileType.HasFlag(TileType.South))
        {
            TileType |= TileType.North;
        }

        if (tiles.Contains(Row + 1, Column) && tiles[Row + 1, Column].TileType.HasFlag(TileType.North))
        {
            TileType |= TileType.South;
        }

        if (tiles.Contains(Row, Column - 1) && tiles[Row, Column - 1].TileType.HasFlag(TileType.East))
        {
            TileType |= TileType.West;
        }

        if (tiles.Contains(Row, Column + 1) && tiles[Row, Column + 1].TileType.HasFlag(TileType.West))
        {
            TileType |= TileType.East;
        }
    }

    public static Tile Parse(int row, int column, char letter)
    {
        var pipe = new Tile
        {
            Row = row,
            Column = column,
            TileType = letter switch
            {
                '|' => TileType.North | TileType.South,
                '-' => TileType.West | TileType.East,
                'L' => TileType.North | TileType.East,
                'J' => TileType.North | TileType.West,
                '7' => TileType.South | TileType.West,
                'F' => TileType.South | TileType.East,
                '.' => TileType.Ground,
                'S' => TileType.Start,
                _ => throw new InvalidOperationException()
            }
        };

        return pipe;
    }
}
