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
        return "111111";

        //var answer = ModifiedDijkstra();

        //return answer.ToString();  // 665
    }

    protected override string SolvePartTwo()
    {
        var answer = ModifiedDijkstra();

        return answer.ToString();
    }

    private int ModifiedDijkstra()
    {
        var currentHeatLoss = 0;

        var visited = new HashSet<int>();

        var crucible = new Crucible(_cityBlocks[0, 0], null, Direction.None, 0, 0);

        var crucibles = new PriorityQueue<Crucible, int>();
        crucibles.Enqueue(crucible, 0);

        while (crucibles.Count > 0)
        {
            crucible = crucibles.Dequeue();
            var hashCode = crucible.GetHashCode();

            if (crucible.CityBlock.Row == _cityBlocks.LastRowIndex && crucible.CityBlock.Column == _cityBlocks.LastColumnIndex)
            {
                currentHeatLoss = crucible.HeatLoss;
                break;
            }
            else if (visited.Contains(hashCode))
            {
                continue;
            }

            visited.Add(hashCode);

            foreach (var neighbor in EnumerateNeighbors(crucible.CityBlock, crucible.Direction))
            {
                var newDirection = DetermineMovementDirection(crucible.CityBlock, neighbor);

                //if (newDirection != crucible.Direction && crucible.Direction != Direction.None && crucible.Steps <= 4)
                //{
                //    continue;
                //}

                //if (newDirection == crucible.Direction && crucible.Steps >= 10)
                //{
                //    continue;
                //}

                if (newDirection != crucible.Direction || crucible.Steps < 3)
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

        //queueItem = new QueueItem(queueItem.State, queueItem, _cityBlocks[queueItem.State.Row, queueItem.State.Column]);

        //var cityBlockList = new List<CityBlock>();
        //while (queueItem != null && queueItem.CityBlock != null)
        //{
        //    cityBlockList.Add(queueItem.CityBlock);
        //    queueItem = queueItem.PreviousQueueItem;
        //}

        //Print2(cityBlockList);

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

    private Direction DetermineMovementDirection(CityBlock previousCityBlock, CityBlock currentCityBlock)
    {
        if (currentCityBlock.Row == previousCityBlock.Row)
        {
            return currentCityBlock.Column > previousCityBlock.Column ? Direction.Right : Direction.Left;
        }
        else
        {
            return currentCityBlock.Row > previousCityBlock.Row ? Direction.Down : Direction.Up;
        }
    }

    private void Print(List<CityBlock> cityBlocksList)
    {
        for (var row = 0; row < _cityBlocks.RowCount; row++)
        {
            for (var column = 0; column < _cityBlocks.ColumnCount; column++)
            {
                var tempCityBlock = cityBlocksList.Find(x => x.Row == row && x.Column == column);
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