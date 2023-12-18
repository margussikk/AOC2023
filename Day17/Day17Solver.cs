using Common;

namespace Day17;

internal class Day17Solver : Solver
{
    private Grid<CityBlock> _cityBlocks = new Grid<CityBlock>(0, 0);

    public Day17Solver() : base(17, "Clumsy Crucible") { }

    protected override void ParseInput(string[] inputLines)
    {
        _cityBlocks = inputLines.MapToGrid((row, column, character) =>
            new CityBlock(row, column, character - '0'));
    }

    protected override string SolvePartOne()
    {
        var answer = ModifiedDijkstra(1, 3);

        return answer.ToString();  // 665
    }

    protected override string SolvePartTwo()
    {
        var answer = ModifiedDijkstra(4, 10);

        return answer.ToString(); // 809
    }

    private int ModifiedDijkstra(int minSteps, int maxSteps)
    {
        var currentHeatLoss = 0;

        var visited = new HashSet<(int, int, Direction, int)>();

        var crucible = new Crucible(_cityBlocks[0, 0], null, Direction.None, 0, 0);

        var crucibles = new PriorityQueue<Crucible, int>();
        crucibles.Enqueue(crucible, 0);

        while (crucibles.Count > 0)
        {
            crucible = crucibles.Dequeue();

            if (crucible.CityBlock.Row == _cityBlocks.LastRowIndex && crucible.CityBlock.Column == _cityBlocks.LastColumnIndex)
            {
                currentHeatLoss = crucible.HeatLoss;
                break;
            }
            else if (visited.Contains(crucible.State))
            {
                continue;
            }

            visited.Add(crucible.State);

            foreach (var neighbor in EnumerateNeighbors(crucible.CityBlock, crucible.Direction))
            {
                var newDirection = neighbor.DirectionFrom(crucible.CityBlock);

                if ((newDirection != crucible.Direction && crucible.Steps >= minSteps) ||
                    ((newDirection == crucible.Direction || crucible.Direction == Direction.None) && crucible.Steps < maxSteps))
                {
                    var newSteps = newDirection != crucible.Direction ? 1 : crucible.Steps + 1;
                    var newHeatLoss = crucible.HeatLoss + neighbor.HeatLoss;

                    var newCrucible = new Crucible(neighbor, crucible, newDirection, newSteps, newHeatLoss);

                    var manhattan = _cityBlocks.LastRowIndex - neighbor.Row +
                                    _cityBlocks.LastColumnIndex - neighbor.Column;

                    crucibles.Enqueue(newCrucible, newHeatLoss + manhattan);
                }
            }
        }

        return currentHeatLoss;
    }

    private IEnumerable<CityBlock> EnumerateNeighbors(CityBlock cityBlock, Direction direction)
    {
        if (direction != Direction.Up && _cityBlocks.InBounds(cityBlock.Row + 1, cityBlock.Column))
        {
            yield return _cityBlocks[cityBlock.Row + 1, cityBlock.Column];
        }

        if (direction != Direction.Down && _cityBlocks.InBounds(cityBlock.Row - 1, cityBlock.Column))
        {
            yield return _cityBlocks[cityBlock.Row - 1, cityBlock.Column];
        }

        if (direction != Direction.Left && _cityBlocks.InBounds(cityBlock.Row, cityBlock.Column + 1))
        {
            yield return _cityBlocks[cityBlock.Row, cityBlock.Column + 1];
        }

        if (direction != Direction.Right && _cityBlocks.InBounds(cityBlock.Row, cityBlock.Column - 1))
        {
            yield return _cityBlocks[cityBlock.Row, cityBlock.Column - 1];
        }
    }

    private void Print(Crucible? crucible)
    {
        var cityBlockList = new List<CityBlock>();

        while (crucible != null && crucible.CityBlock != null)
        {
            cityBlockList.Add(crucible.CityBlock);
            crucible = crucible.PreviousCrucible;
        }

        for (var row = 0; row < _cityBlocks.RowCount; row++)
        {
            for (var column = 0; column < _cityBlocks.ColumnCount; column++)
            {
                var tempCityBlock = cityBlockList.Find(x => x.Row == row && x.Column == column);
                if (tempCityBlock != null)
                {
                    Console.Write('.');
                }
                else
                {
                    Console.Write($"{_cityBlocks[row, column].HeatLoss}");
                }
            }
            Console.WriteLine();
        }
    }
}