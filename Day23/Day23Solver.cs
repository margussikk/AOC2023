using Common;

namespace Day23;

internal class Day23Solver : Solver
{
    private Vertex? _startVertex = null;

    private Vertex? _endVertex = null;

    public Day23Solver() : base(23, "A Long Walk") { }

    protected override void ParseInput(string[] inputLines)
    {
        var tiles = inputLines.MapToGrid((row, column, character) => character switch
        {
            '.' => new Tile(row, column, TileType.Path),
            '#' => new Tile(row, column, TileType.Forest),
            '^' => new Tile(row, column, TileType.SlopeUp),
            'v' => new Tile(row, column, TileType.SlopeDown),
            '<' => new Tile(row, column, TileType.SlopeLeft),
            '>' => new Tile(row, column, TileType.SlopeRight),
            _ => throw new InvalidOperationException()
        });

        var startTile = tiles.EnumerateRow(0).First(x => x.TileType == TileType.Path);
        var endTile = tiles.EnumerateRow(tiles.RowCount - 1).First(x => x.TileType == TileType.Path);

        var vertices = BuildVertices(tiles, startTile, endTile);

        _startVertex = vertices.First(x => x.Tile == startTile);
        _endVertex = vertices.First(x => x.Tile == endTile);
    }

    protected override string SolvePartOne()
    {
        var answer = GetLongestPath(false);

        return answer.ToString(); // 2170
    }

    protected override string SolvePartTwo()
    {
        var answer = GetLongestPath(true);

        return answer.ToString(); // 6502
    }

    private int GetLongestPath(bool ignoreSlopes)
    {
        var longestPath = 0;

        if (_startVertex == null)
        {
            throw new InvalidOperationException("Start vertex is null");
        }

        if (_endVertex == null)
        {
            throw new InvalidOperationException("End vertex is null");
        }

        var vertexHikerStack = new Stack<VertexHiker>();

        var vertexHiker = new VertexHiker(_startVertex, 0, 0);
        vertexHikerStack.Push(vertexHiker);

        while (vertexHikerStack.Count > 0)
        {
            vertexHiker = vertexHikerStack.Pop();

            if (vertexHiker.Vertex == _endVertex)
            {
                longestPath = int.Max(longestPath, vertexHiker.Distance);
                continue;
            }

            if ((vertexHiker.VisitedBitMask & vertexHiker.Vertex.BitMask) == vertexHiker.Vertex.BitMask)
            {
                continue;
            }

            vertexHiker.VisitedBitMask |= vertexHiker.Vertex.BitMask;

            foreach(var edge in vertexHiker.Vertex.Edges)
            {
                if (edge.StartVertex == vertexHiker.Vertex || ignoreSlopes)
                {
                    var endVertex = edge.StartVertex == vertexHiker.Vertex ? edge.EndVertex : edge.StartVertex;

                    var newVertexHiker = new VertexHiker(endVertex, vertexHiker.Distance + edge.Distance, vertexHiker.VisitedBitMask);
                    vertexHikerStack.Push(newVertexHiker);
                }
            }
        }

        return longestPath;
    }


    private static List<Vertex> BuildVertices(Grid<Tile> tiles, Tile startTile, Tile endTile)
    {
        // Find vertices
        var vertices = tiles.EnumerateAll()
            .Where(tile =>
                tile == startTile ||
                tile == endTile ||
                (tile.TileType != TileType.Forest &&
                 tiles.EnumerateSideNeighbors(tile.Row, tile.Column)
                      .Count(x => x.TileType != TileType.Forest) > 2))  // Has 3 or more open paths around
            .Select((tile, index) => new Vertex(index, tile))
            .ToList();

        var vertexLocations = vertices.Select(x => x.Tile.Coordinate).ToHashSet();

        var visited = new HashSet<GridCoordinate>();
        foreach (var tile in vertices.Select(x => x.Tile))
        {
            var tileHikers = new Stack<TileHiker>();

            if (IsPathTile(tile, -1, 0))
            {
                var tileHiker = new TileHiker(tile, tile, Direction.Up, 0);
                tileHikers.Push(tileHiker);
            }

            if (IsPathTile(tile, 1, 0))
            {
                var tileHiker = new TileHiker(tile, tile, Direction.Down, 0);
                tileHikers.Push(tileHiker);
            }

            if (IsPathTile(tile, 0, -1))
            {
                var tileHiker = new TileHiker(tile, tile, Direction.Left, 0);
                tileHikers.Push(tileHiker);
            }

            if (IsPathTile(tile, 0, 1))
            {
                var tileHiker = new TileHiker(tile, tile, Direction.Right, 0);
                tileHikers.Push(tileHiker);
            }

            while (tileHikers.Count > 0)
            {
                var tileHiker = tileHikers.Pop();

                if (vertexLocations.Contains(tileHiker.CurrentTile.Coordinate) && tileHiker.CurrentTile != tileHiker.StartTile)
                {
                    var startVertex = vertices.First(x => x.Tile == tileHiker.StartTile);
                    var endVertex = vertices.First(x => x.Tile == tileHiker.CurrentTile);

                    var entranceTileType = tileHiker.Direction switch
                    {
                        Direction.Down => tiles[tileHiker.CurrentTile.Row - 1, tileHiker.CurrentTile.Column].TileType,
                        Direction.Up => tiles[tileHiker.CurrentTile.Row + 1, tileHiker.CurrentTile.Column].TileType,
                        Direction.Left => tiles[tileHiker.CurrentTile.Row, tileHiker.CurrentTile.Column + 1].TileType,
                        Direction.Right => tiles[tileHiker.CurrentTile.Row, tileHiker.CurrentTile.Column - 1].TileType,
                        _ => throw new InvalidOperationException()
                    };

                    if ((tileHiker.Direction == Direction.Down && (entranceTileType is TileType.SlopeDown or TileType.Path)) ||
                        (tileHiker.Direction == Direction.Up && (entranceTileType is TileType.SlopeUp or TileType.Path)) ||
                        (tileHiker.Direction == Direction.Left && (entranceTileType is TileType.SlopeLeft or TileType.Path)) ||
                        (tileHiker.Direction == Direction.Right && (entranceTileType is TileType.SlopeRight or TileType.Path)))
                    {
                        // Vertex1 is start, Vertex2 is end. Do nothing.
                    }
                    else
                    {
                        // Vertex2 is start, Vertex1 is end. Swap vertices.
                        (startVertex, endVertex) = (endVertex, startVertex);
                    }

                    var edge = new Edge(startVertex, endVertex, tileHiker.Distance);
                    startVertex.Edges.Add(edge);
                    endVertex.Edges.Add(edge);

                    foreach (var neighboringTile in EnumerateNeighboringTiles(tileHiker.CurrentTile, tileHiker.Direction))
                    {
                        if (neighboringTile.TileType == TileType.Forest)
                        {
                            continue;
                        }

                        var direction = neighboringTile.DirectionFrom(tileHiker.CurrentTile);

                        var newTileHiker = new TileHiker(tileHiker.CurrentTile, neighboringTile, direction, 1);

                        tileHikers.Push(newTileHiker);
                    }

                    visited.Add(tileHiker.CurrentTile.Coordinate);
                    continue;
                }

                if (visited.Contains(tileHiker.CurrentTile.Coordinate))
                {
                    continue;
                }

                visited.Add(tileHiker.CurrentTile.Coordinate);

                // Find neighbors
                foreach (var neighboringTile in EnumerateNeighboringTiles(tileHiker.CurrentTile, tileHiker.Direction))
                {
                    if (neighboringTile.TileType == TileType.Forest)
                    {
                        continue;
                    }

                    var direction = neighboringTile.DirectionFrom(tileHiker.CurrentTile);

                    var newTileHiker = new TileHiker(tileHiker.StartTile, neighboringTile, direction, tileHiker.Distance + 1);
                    tileHikers.Push(newTileHiker);
                }
            }
        }

        return vertices;

        bool IsPathTile(Tile tile, int rowDelta, int columnDelta)
        {
            var newRow = tile.Row + rowDelta;
            var newColumn = tile.Column + columnDelta;

            return tiles.InBounds(newRow, newColumn) && tiles[newRow, newColumn].TileType != TileType.Forest;
        }

        IEnumerable<Tile> EnumerateNeighboringTiles(Tile tile, Direction direction)
        {
            if (direction != Direction.Up && tiles.InBounds(tile.Row + 1, tile.Column))
            {
                yield return tiles[tile.Row + 1, tile.Column];
            }

            if (direction != Direction.Down && tiles.InBounds(tile.Row - 1, tile.Column))
            {
                yield return tiles[tile.Row - 1, tile.Column];
            }

            if (direction != Direction.Left && tiles.InBounds(tile.Row, tile.Column + 1))
            {
                yield return tiles[tile.Row, tile.Column + 1];
            }

            if (direction != Direction.Right && tiles.InBounds(tile.Row, tile.Column - 1))
            {
                yield return tiles[tile.Row, tile.Column - 1];
            }
        }
    }
}