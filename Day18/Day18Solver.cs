using Common;

namespace Day18;

internal class Day18Solver : Solver
{
    private List<Instruction> _part1Instructions = new List<Instruction>();
    private List<Instruction> _part2Instructions = new List<Instruction>();

    public Day18Solver() : base(18, "Lavaduct Lagoon") { }

    protected override void ParseInput(string[] inputLines)
    {
        _part1Instructions = inputLines
            .Select(Instruction.Part1Parse)
            .ToList();

        _part2Instructions = inputLines
            .Select(Instruction.Part2Parse)
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var answer = CalculateLagoonCapacity(_part1Instructions);

        return answer.ToString(); // 35991
    }

    protected override string SolvePartTwo()
    {
        var answer = CalculateLagoonCapacity(_part2Instructions);

        return answer.ToString(); // 54058824661845
    }

    private static long CalculateLagoonCapacity(List<Instruction> instructions)
    {
        // Create vertices from instructions
        var vertices = new List<Vertex>();
        var vertex = new Vertex(0, 0);

        foreach (var instruction in instructions)
        {
            vertex = instruction.Direction switch
            {
                Direction.Right => new Vertex(vertex.Row, vertex.Column + instruction.Distance),
                Direction.Left => new Vertex(vertex.Row, vertex.Column - instruction.Distance),
                Direction.Up => new Vertex(vertex.Row - instruction.Distance, vertex.Column),
                Direction.Down => new Vertex(vertex.Row + instruction.Distance, vertex.Column),
                _ => throw new InvalidOperationException()
            };

            vertices.Add(vertex);
        }

        // Shoelace formula (https://en.wikipedia.org/wiki/Shoelace_formula)
        // Area = |x1y2 - y1x2 + x2y3 - y2x3 + ... + xny1 - ynx1| / 2
        var sum = Enumerable
            .Range(0, vertices.Count)
            .Select(index =>
            {
                var index2 = (index + 1) % vertices.Count;

                return vertices[index].Row * vertices[index2].Column - vertices[index].Column * vertices[index2].Row;
            })
            .Sum();
        var area = Math.Abs(sum) / 2;

        // Pick's theorem (https://en.wikipedia.org/wiki/Pick%27s_theorem)
        // Area = [interior points] + [boundary points] / 2 - 1
        // Area - [boundary points] / 2 + 1  = [interior points]
        var boundaryPoints = instructions.Sum(x => x.Distance);
        var interiorPoints = area - (boundaryPoints / 2) + 1;

        return boundaryPoints + interiorPoints;
    }
}