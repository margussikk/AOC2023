using System.Text.RegularExpressions;

namespace Day19;

internal partial class Part // partial because of Regex
{
    public int[] Ratings { get; set; } = new int[4];

    public static Part Parse(string input)
    {
        var matches = InputRegex().Matches(input);
        if (matches.Count == 1)
        {
            return new Part
            {
                Ratings =
                [
                    int.Parse(matches[0].Groups[1].Value), // X
                    int.Parse(matches[0].Groups[2].Value), // M
                    int.Parse(matches[0].Groups[3].Value), // A
                    int.Parse(matches[0].Groups[4].Value), // S
                ]
            };
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    [GeneratedRegex("x=(\\d+),m=(\\d+),a=(\\d+),s=(\\d+)")]
    private static partial Regex InputRegex();
}
