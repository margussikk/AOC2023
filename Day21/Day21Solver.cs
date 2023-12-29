using Common;

namespace Day21;

//                                                      
//                              +-----+-----+-----+
//                              |     |  0  |     |
//                              |     | 000 |     |
//                              |    1|00000|1    |
//                              |   11|00000|11   |
//                              |  111|00000|111  |
//                        +-----+-----+-----+-----+-----+
//                        |     |  222|33333|222  |     |
//                        |     | 2222|33333|2222 |     |
//                        |    1|22222|33333|22222|1    |           Solution works by filling every type of grid to count how many garden spots are reachable in that grid.
//                        |   11|22222|33333|22222|11   |           Secondly amount of different grids and garden spots in them are used to calculate total reachable garden spots count.
//                        |  111|22222|33333|22222|111  |
//                        +-----+-----+-----+-----+-----+
//                        |  000|33333|44444|33333|000  |           0 = top, left, right, bottom
//                        | 0000|33333|44444|33333|0000 |           1 = smallTopLeft, smallTopRight, smallBottomLeft, smallBottomRight
//                        |00000|33333|44444|33333|00000|           2 = largeTopLeft, largeTopRight, largeBottomLeft, largeBottomRight
//                        | 0000|33333|44444|33333|0000 |           3 = odd
//                        |  000|33333|44444|33333|000  |           4 = even
//                        +-----+-----+-----+-----+-----+
//                        |  111|22222|33333|22222|111  |
//                        |   11|22222|33333|22222|11   |
//                        |    1|22222|33333|22222|1    |
//                        |     | 2222|33333|2222 |     |
//                        |     |  222|33333|222  |     |
//                        +-----+-----+-----+-----+-----+
//                              |  111|00000|111  |
//                              |   11|00000|11   |
//                              |    1|00000|1    |
//                              |     | 000 |     |
//                              |     |  0  |     |
//                              +-----+-----+-----+


internal class Day21Solver : Solver
{
    private Grid<Tile> _tiles = new Grid<Tile>(0, 0);

    public Day21Solver() : base(21, "Step Counter") { }

    protected override void ParseInput(string[] inputLines)
    {
        _tiles = inputLines.MapToGrid(
            (row, column, characher) => characher switch
            {
                'S' => new Tile(row, column, TileType.StartingPosition),
                '.' => new Tile(row, column, TileType.GardenPlot),
                '#' => new Tile(row, column, TileType.Rock),
                _ => throw new InvalidOperationException()
            });
    }

    protected override string SolvePartOne()
    {
        var startingTile = _tiles.EnumerateAll().First(x => x.TileType == TileType.StartingPosition);
        var answer = CountReachableGardenSpots(startingTile.Row, startingTile.Column, 64);

        return answer.ToString(); // 3716
    }

    protected override string SolvePartTwo()
    {
        var startingTile = _tiles.EnumerateAll().First(x => x.TileType == TileType.StartingPosition);
        if (startingTile.Row != 65 || startingTile.Column != 65)
        {
            throw new InvalidOperationException("This solution expects Start to be at 65,65");
        }

        var stepCount = 26501365L; // 202300 * 131 + 65
        var gridCount = (stepCount - 65) / _tiles.RowCount; // Assume grids are squares

        var firstRow = 0;
        var lastRow = _tiles.LastRowIndex;
        var firstColumn = 0;
        var lastColumn = _tiles.LastColumnIndex;
        var middleRow = _tiles.RowCount / 2;
        var middleColumn = _tiles.ColumnCount / 2;


        // Fill every type of grid to count possible reached garden spots. Some grids are filled fully,
        // some partally using various starting points and step counts. See diagram above.
        var even = CountReachableGardenSpots(middleRow, middleColumn, 129);
        var odd = CountReachableGardenSpots(middleRow, middleColumn, 130);

        var left = CountReachableGardenSpots(middleRow, lastColumn, 130);
        var right = CountReachableGardenSpots(middleRow, firstColumn, 130);
        var top = CountReachableGardenSpots(lastRow, middleColumn, 130);
        var bottom = CountReachableGardenSpots(firstRow, middleColumn, 130);

        var smallTopLeft = CountReachableGardenSpots(lastRow, lastColumn, 64);
        var smallTopRight = CountReachableGardenSpots(lastRow, firstColumn, 64);
        var smallBottomLeft = CountReachableGardenSpots(firstRow, lastColumn, 64);
        var smallBottomRight = CountReachableGardenSpots(firstRow, firstColumn, 64);

        var largeTopLeft = CountReachableGardenSpots(lastRow, lastColumn, 130 + 65);
        var largeTopRight = CountReachableGardenSpots(lastRow, firstColumn, 130 + 65);
        var largeBottomLeft = CountReachableGardenSpots(firstRow, lastColumn, 130 + 65);
        var largeBottomRight = CountReachableGardenSpots(firstRow, firstColumn, 130 + 65);

        var answer = (gridCount - 1) * (gridCount - 1) * even +
            (gridCount * gridCount) * odd +
            top + left + right + bottom +
            gridCount * (smallTopLeft + smallTopRight + smallBottomLeft + smallBottomRight) +
            (gridCount - 1) * (largeTopLeft + largeTopRight + largeBottomLeft + largeBottomRight);

        return answer.ToString(); // 616583483179597
    }

    private long CountReachableGardenSpots(int startRow, int startColumn, int steps)
    {
        var visited = new HashSet<(int, int)>[2]
        {
            [], // Even
            [], // Odd
        };
        var queue = new Queue<Gardener>();

        var gardener = new Gardener(startRow, startColumn, 0);
        queue.Enqueue(gardener);

        while (queue.Count > 0)
        {
            gardener = queue.Dequeue();

            var state = (gardener.Row, gardener.Column);
            if (gardener.Steps > steps || visited[gardener.Steps % 2].Contains(state))
            {
                continue;
            }

            visited[gardener.Steps % 2].Add(state);

            foreach(var neighborTile in _tiles.EnumerateSideNeighbors(gardener.Row, gardener.Column).Where(t => t.TileType != TileType.Rock))
            {
                var newGardener = new Gardener(neighborTile.Row, neighborTile.Column, gardener.Steps + 1);
                queue.Enqueue(newGardener);
            }
        }

        return visited[steps % 2].Count;
    }
}