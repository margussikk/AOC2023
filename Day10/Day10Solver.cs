using Common;
using System.Text;

namespace Day10;

internal partial class Day10Solver : Solver
{
    private Grid<Tile> _tiles = new Grid<Tile>(0, 0);

    private Tile? _startTile;

    public Day10Solver() : base(10, "Pipe Maze") { }

    protected override void ParseInput(string[] inputLines)
    {
        _tiles = new Grid<Tile>(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < _tiles.RowCount; row++)
        {
            for (var column = 0; column < _tiles.ColumnCount; column++)
            {
                var tile = Tile.Parse(row, column, inputLines[row][column]);
                if (tile.TileType.HasFlag(TileType.Start))
                {
                    _startTile = tile;
                }

                _tiles[row, column] = tile;
            }
        }
    }

    protected override string SolvePartOne()
    {
        if (_startTile == null)
        {
            throw new NotImplementedException();
        }

        _startTile.DetermineStartTileType(_tiles);

        var tile = _startTile;

        var tileEntrancePart = new TileType[] { TileType.North, TileType.East, TileType.South, TileType.West }
            .First(x => tile.TileType.HasFlag(x));

        var steps = 0L;
        do
        {
            tile.IsAlongTheLoop = true; // Used in part two

            // Remove Start info and entrance info and determine where to exit
            var tileExitPart = tile.TileType & ~(TileType.Start | tileEntrancePart);
            switch (tileExitPart)
            {
                case TileType.North:
                    tile = _tiles[tile.Row - 1, tile.Column];
                    tileEntrancePart = TileType.South;
                    break;
                case TileType.East:
                    tile = _tiles[tile.Row, tile.Column + 1];
                    tileEntrancePart = TileType.West;
                    break;
                case TileType.South:
                    tile = _tiles[tile.Row + 1, tile.Column];
                    tileEntrancePart = TileType.North;
                    break;
                case TileType.West:
                    tile = _tiles[tile.Row, tile.Column - 1];
                    tileEntrancePart = TileType.East;
                    break;
                default: 
                    throw new InvalidOperationException();
            }

            steps++;
        } while (!tile.TileType.HasFlag(TileType.Start));

        var answer = steps / 2; // Half way is the furthest distance

        return answer.ToString(); // 7086
    }    

    protected override string SolvePartTwo()
    {
        // Part one needs to be run earlier. Part one marks the loop
        var enclosedTiles = 0;

        for (var row = 0; row < _tiles.RowCount; row++)
        {
            var northPart = false;
            var southPart = false;

            for (var column = 0; column < _tiles.ColumnCount; column++)
            {
                var tile = _tiles[row, column];
                if (tile.IsAlongTheLoop)
                {
                    if (tile.TileType.HasFlag(TileType.North))
                    {
                        northPart = !northPart;
                    }

                    if (tile.TileType.HasFlag(TileType.South))
                    {
                        southPart = !southPart;
                    }
                }
                else if (northPart && southPart)
                {
                    enclosedTiles++;
                }
            }
        }

        return enclosedTiles.ToString(); // 317
    }

    private void PrintGrid() // For debugging
    {
        Console.OutputEncoding = Encoding.UTF8;

        for (var row = 0; row < _tiles.RowCount; row++)
        {
            for (var column = 0; column < _tiles.ColumnCount; column++)
            {
                string symbol;

                if (_tiles[row, column].TileType.HasFlag(TileType.Start))
                {
                    symbol = "S";
                }
                else
                {
                    symbol = _tiles[row, column].TileType switch
                    {
                        TileType.North | TileType.South => "\u2503",
                        TileType.West | TileType.East => "\u2501",
                        TileType.North | TileType.East => "\u2517",
                        TileType.North | TileType.West => "\u251b",
                        TileType.South | TileType.East => "\u250f",
                        TileType.South | TileType.West => "\u2513",
                        TileType.Ground => " ",
                        _ => throw new InvalidOperationException(),
                    };
                }

                Console.Write(symbol);
            }
            Console.WriteLine();
        }
    }
}