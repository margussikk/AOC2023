using Common;

namespace Day16;

internal class Day16Solver : Solver
{
    private Grid<Tile> _tiles = new Grid<Tile>(0, 0);

    public Day16Solver() : base(16, "The Floor Will Be Lava") { }

    protected override void ParseInput(string[] inputLines)
    {
        _tiles = new Grid<Tile>(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < _tiles.RowCount;  row++)
        {
            for (var column = 0; column < _tiles.ColumnCount; column++)
            {
                _tiles[row, column] = inputLines[row][column] switch
                {
                    '.' => Tile.EmptySpace,
                    '/' => Tile.PositiveSlopeMirror,
                    '\\' => Tile.NegativeSlopeMirror,
                    '-' => Tile.HorizontalSplitter,
                    '|' => Tile.VerticalSplitter,
                    _ => throw new InvalidOperationException()
                };
            }
        }
    }

    protected override string SolvePartOne()
    {
        var beam = new Beam(0, 0, Direction.Right);

        var answer = CountEnergizedTiles(beam);

        return answer.ToString(); // 7870
    }

    protected override string SolvePartTwo()
    {
        var energizedTiles = new List<int>();

        for (var i = 0; i < _tiles.RowCount; i++) // NB! Assume that the puzzle input is a square and RowCount == ColumnCount
        {
            // Right
            var rightBeam = new Beam(i, 0, Direction.Right);
            energizedTiles.Add(CountEnergizedTiles(rightBeam));

            // Left
            var leftBeam = new Beam(i, _tiles.ColumnCount - 1, Direction.Left);
            energizedTiles.Add(CountEnergizedTiles(leftBeam));

            // Down
            var downBeam = new Beam(0, i, Direction.Down);
            energizedTiles.Add(CountEnergizedTiles(downBeam));

            // Up
            var upBeam = new Beam(_tiles.RowCount - 1, i, Direction.Up);
            energizedTiles.Add(CountEnergizedTiles(upBeam));
        }

        var answer = energizedTiles.Max().ToString();

        return answer.ToString(); // 8143
    }

    private int CountEnergizedTiles(Beam beam)
    {
        var visitedTiles = new Grid<Direction>(_tiles.RowCount, _tiles.ColumnCount);
        var beams = new Stack<Beam>();
        
        beams.Push(beam);

        while (beams.Count > 0)
        {
            beam = beams.Pop();

            while (visitedTiles.InBounds(beam.Row, beam.Column))
            {
                var tile = _tiles[beam.Row, beam.Column];

                // Check visited
                var directionMask = tile switch
                {
                    Tile.EmptySpace or
                    Tile.HorizontalSplitter or
                    Tile.VerticalSplitter =>
                        beam.Direction switch
                        {
                            Direction.Left or Direction.Right => Direction.Left | Direction.Right,
                            Direction.Up or Direction.Down => Direction.Up | Direction.Down,
                            _ => throw new InvalidOperationException()
                        },
                    Tile.PositiveSlopeMirror =>
                        beam.Direction switch
                        {
                            Direction.Right or Direction.Down => Direction.Left | Direction.Up,
                            Direction.Left or Direction.Up => Direction.Right | Direction.Down,
                            _ => throw new InvalidOperationException()
                        },
                    Tile.NegativeSlopeMirror =>
                        beam.Direction switch
                        {
                            Direction.Right or Direction.Up => Direction.Left | Direction.Down,
                            Direction.Left or Direction.Down => Direction.Right | Direction.Up,
                            _ => throw new InvalidOperationException()
                        },
                    _ => throw new NotImplementedException()
                };

                if (visitedTiles[beam.Row, beam.Column].HasFlag(directionMask))
                {
                    // Already visited the tile in that general direction (i.e. left to right is the same as right to left)
                    break;
                }
                                
                visitedTiles[beam.Row, beam.Column] |= directionMask;

                // Process beam's movement
                if (tile is Tile.PositiveSlopeMirror or Tile.NegativeSlopeMirror)
                {
                    if ((tile == Tile.PositiveSlopeMirror && beam.IsHorizontal) ||
                        (tile == Tile.NegativeSlopeMirror && beam.IsVertical))
                    {
                        beam.TurnLeft();
                    }
                    else
                    {
                        beam.TurnRight();
                    }
                }
                else if (tile is Tile.HorizontalSplitter or Tile.VerticalSplitter)
                {
                    if ((tile == Tile.HorizontalSplitter && beam.IsHorizontal) ||
                        (tile == Tile.VerticalSplitter && beam.IsVertical))
                    {
                        beam.ContinueSameDirection();
                    }
                    else
                    {
                        // New beam turns left
                        var clone = beam.Clone();
                        clone.TurnLeft();
                        beams.Push(clone);

                        // Current beam turns right
                        beam.TurnRight();
                    }
                }
                else // Empty space
                {
                    beam.ContinueSameDirection();
                }
            }
        }

        return visitedTiles.EnumerateAll().Count(x => x != 0);
    }
}