using Common;
using System.Text;

namespace Day01;

internal partial class Day01Solver : Solver
{
    private readonly SpelledDigit[] _spelledDigits =
    [
        new SpelledDigit(1, "one"),
        new SpelledDigit(2, "two"),
        new SpelledDigit(3, "three"),
        new SpelledDigit(4, "four"),
        new SpelledDigit(5, "five"),
        new SpelledDigit(6, "six"),
        new SpelledDigit(7, "seven"),
        new SpelledDigit(8, "eight"),
        new SpelledDigit(9, "nine")
    ];

    private string[] _inputLines = [];

    public Day01Solver(): base(1, "Trebuchet?!") { }

    protected override void ParseInput(string[] inputLines)
    {
        _inputLines = inputLines;
    }

    protected override string SolvePartOne()
    {
        var calibrationValueSum = _inputLines
            .Select(CalculateCalibrationValue)
            .Sum();

        return calibrationValueSum.ToString(); // 54605
    }

    protected override string SolvePartTwo()
    {
        var calibrationValueSum = _inputLines
            .Select(StandardizeLine)
            .Select(CalculateCalibrationValue)
            .Sum();

        return calibrationValueSum.ToString(); // 55429
    }

    private string StandardizeLine(string line)
    {
        var stringBuilder = new StringBuilder();
        
        for (var index = 0; index < line.Length; index++)
        {
            var spelledDigit = Array.Find(_spelledDigits, sd =>
                (index + sd.Length) <= line.Length &&
                MemoryExtensions.Equals(line.AsSpan(index, sd.Length), sd.Word, StringComparison.Ordinal));

            if (spelledDigit != null)
            {
                stringBuilder.Append(spelledDigit.Digit);
            }
            else
            {
                stringBuilder.Append(line[index]);
            }
        }

        return stringBuilder.ToString();
    }

    private static long CalculateCalibrationValue(string line)
    {
        var digits = line.Where(char.IsDigit).ToArray();
        if (digits.Length > 0)
        {
            return long.Parse($"{digits[0]}{digits[^1]}");
        }

        return 0;
    }
}
