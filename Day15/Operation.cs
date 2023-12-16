using System.Text.RegularExpressions;

namespace Day15;

internal abstract partial class Operation
{
    public string Label { get; private set; }

    protected Operation(string label)
    {
        Label = label;
    }

    public static Operation Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count == 1)
        {
            if (matches[0].Groups[2].Value == "=")
            {
                var label = matches[0].Groups[1].Value;
                var focalLength = int.Parse(matches[0].Groups[3].Value);

                return new ReplaceLensOperation(label, focalLength);
            }
            else if (matches[0].Groups[2].Value == "-")
            {
                var label = matches[0].Groups[1].Value;

                return new RemoveLensOperation(label);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    [GeneratedRegex("([a-z]+)([=-])([\\d]?)")]
    private static partial Regex InputLineRegex();
}
