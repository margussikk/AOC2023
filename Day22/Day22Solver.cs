using Common;

namespace Day22;

internal class Day22Solver : Solver
{
    private List<Brick> _bricks = new List<Brick>();

    public Day22Solver() : base(22, "Sand Slabs") { }

    protected override void ParseInput(string[] inputLines)
    {
        _bricks = inputLines
            .Select((line, index) => Brick.Parse(index + 1, line)) // "real" brick ids start at 1, 0 means ground
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var bricks = SettleBricks(_bricks);

        var answer = bricks
            .Count(b1 => b1.Supports.TrueForAll(b2 => b2.SupportedBy.Count > 1)); // NB! TrueForAll returns true if list is empty

        return answer.ToString(); // 434
    }

    protected override string SolvePartTwo()
    {
        var answer = 0;

        var bricks = SettleBricks(_bricks);

        foreach (var brick in bricks)
        {
            var fallenBrickIds = new HashSet<int>();
            var fallenBricksQueue = new Queue<Brick>();

            fallenBricksQueue.Enqueue(brick);
            while(fallenBricksQueue.Count > 0)
            {
                var fallenBrick = fallenBricksQueue.Dequeue();
                fallenBrickIds.Add(fallenBrick.Id);

                foreach (var supportedBrick in fallenBrick.Supports)
                {
                    if (supportedBrick.SupportedBy.Exists(x => !fallenBrickIds.Contains(x.Id)))
                    {
                        // Brick which is supported by current brick is also supported by some other brick.
                        continue;
                    }

                    fallenBricksQueue.Enqueue(supportedBrick);
                }
            }

            answer += fallenBrickIds.Count - 1; // - 1 = Don't count ground brick
        }

        return answer.ToString(); // 61209
    }

    private List<Brick> SettleBricks(List<Brick> bricks)
    {
        bricks = bricks
            .Select(b => b.CleanClone())
            .OrderBy(x => x.Start.Z)
            .ToList();

        var maxX = bricks.Max(b => b.End.X);
        var maxY = bricks.Max(b => b.End.Y);

        // Keep track of the highest brick that's "open". Add pseudo ground bricks.
        var groundBrick = Brick.CreateGroundBrick(maxX, maxY);

        var stackedBricks = new Brick[maxX + 1, maxY + 1];
        for (var x = 0; x < stackedBricks.GetLength(0); x++)
        {
            for (var y = 0; y < stackedBricks.GetLength(1); y++)
            {
                stackedBricks[x, y] = groundBrick;
            }
        }

        foreach (var brick in bricks)
        {
            // Find how brick low can drop
            var maxHeight = int.MinValue;

            for (var x = brick.Start.X; x <= brick.End.X; x++)
            {
                for (var y = brick.Start.Y; y <= brick.End.Y; y++)
                {
                    maxHeight = int.Max(maxHeight, stackedBricks[x, y].End.Z);
                }
            }

            brick.DropToZ(maxHeight + 1);

            for (var x = brick.Start.X; x <= brick.End.X; x++)
            {
                for (var y = brick.Start.Y; y <= brick.End.Y; y++)
                {
                    var stackedBrick = stackedBricks[x, y];

                    if (stackedBrick.End.Z == maxHeight)
                    {
                        brick.AddSupportedBy(stackedBrick);
                        stackedBrick.AddSupports(brick);
                    }

                    stackedBricks[x, y] = brick;
                }
            }
        }

        return bricks;
    }
}