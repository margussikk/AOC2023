using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21;

internal class Tile
{
    public int Row { get; set; }
    public int Column { get; set; }
    public TileType TileType { get; set; }

    public Tile(int row, int column, TileType tileType)
    {
        Row = row;
        Column = column;
        TileType = tileType;
    }
}
