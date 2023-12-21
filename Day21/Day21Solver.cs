using Common;

namespace Day21;

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
        var steps = 64;

        var visited = new Dictionary<int, HashSet<(int, int)>>();
        var queue = new Queue<Gardener>();

        var startingTile = _tiles.EnumerateAll().First(x => x.TileType == TileType.StartingPosition);
        var gardener = new Gardener(startingTile.Row, startingTile.Column, 0);

        queue.Enqueue(gardener);

        while(queue.Count > 0)
        {
            gardener = queue.Dequeue();

            if(!visited.TryGetValue(gardener.Steps, out var visitedMap))
            {
                visitedMap = new HashSet<(int, int)>();
                visited[gardener.Steps] = visitedMap;
            }

            var state = (gardener.Row, gardener.Column);
            if (visitedMap.Contains(state) || gardener.Steps > steps)
            {
                continue;
            }

            visitedMap.Add(state);

            var neighbors = _tiles.EnumerateSideNeighbors(gardener.Row, gardener.Column);
            foreach (var neighbor in neighbors)
            {
                if (neighbor.TileType == TileType.GardenPlot || neighbor.TileType == TileType.StartingPosition)
                {
                    var newGardener = new Gardener(neighbor.Row, neighbor.Column, gardener.Steps + 1);
                    queue.Enqueue(newGardener);
                }
            }
        }

        var answer = visited[steps].Count.ToString();

        return answer; // 3716
    }

    protected override string SolvePartTwo()
    {
        return "TODO";
    }
}