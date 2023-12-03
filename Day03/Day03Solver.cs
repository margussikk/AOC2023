using Common;
using System.Text;

namespace Day03;

internal class Day03Solver : Solver
{
    private readonly List<EnginePart> _engineParts = new List<EnginePart>();

    public Day03Solver() : base(3, "Gear Ratios") { }

    protected override void ParseInput(string[] inputLines)
    {
        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                if (inputLines[row][column] == '.')
                {
                    continue;
                }

                var startColumn = column;
                var stringBuilder = new StringBuilder();

                while(column < inputLines[row].Length)
                {
                    stringBuilder.Append(inputLines[row][column]);

                    // Peek the next character and decide whether to continue building current number value or not
                    if (char.IsDigit(inputLines[row][column]) && column + 1 < inputLines[row].Length && char.IsDigit(inputLines[row][column + 1]))
                    {
                        // Continue building current number value
                        column++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (stringBuilder.Length > 0)
                {
                    var engineObject = new EnginePart
                    {
                        Row = row,
                        Column = startColumn,
                        Value = stringBuilder.ToString(),
                    };

                    _engineParts.Add(engineObject);
                }
            }
        }        
    }

    protected override string SolvePartOne()
    {
        var numberParts = _engineParts.Where(x => x.IsNumberPart).ToList();
        var symbolParts = _engineParts.Where(x => x.IsSymbolPart).ToList();

        var answer = numberParts
            .Where(numberPart => symbolParts
                .Exists(symbolPart => numberPart.IsAdjacentTo(symbolPart)))
            .Sum(numberPart => long.Parse(numberPart.Value))
            .ToString();

        return answer; // 527144
    }

    protected override string SolvePartTwo()
    {
        var numberParts = _engineParts.Where(x => x.IsNumberPart).ToList();
        var gearParts = _engineParts.Where(x => x.IsGearPart).ToList();

        var answer = gearParts
            .Select(gearPart => numberParts
                .Where(numberPart => numberPart.IsAdjacentTo(gearPart))
                .ToList())
            .Where(adjacentNumberParts => adjacentNumberParts.Count == 2)
            .Sum(adjacentNumberParts => long.Parse(adjacentNumberParts[0].Value) * long.Parse(adjacentNumberParts[1].Value))
            .ToString();

        return answer; // 81463996
    }
}
