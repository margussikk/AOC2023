namespace Day19;

internal abstract class Rule
{
    public string WorkflowName { get; private set; }

    protected Rule(string workflowName)
    {
        WorkflowName = workflowName;
    }

    public abstract bool Matches(Part part);

    public static Rule Parse(string input)
    {
        var destinationSplits = input.Split(':');
        if (destinationSplits.Length == 2)
        {
            var lessThanSplits = destinationSplits[0].Split('<');
            if (lessThanSplits.Length == 2)
            {
                var partRating = ParsePartRating(lessThanSplits[0][0]);
                var value = int.Parse(lessThanSplits[1]);

                return new LessThanRule(partRating, value, destinationSplits[1]);
            }
            else
            {
                var greaterThanSplits = destinationSplits[0].Split('>');

                var partRating = ParsePartRating(greaterThanSplits[0][0]);
                var value = int.Parse(greaterThanSplits[1]);

                return new GreaterThanRule(partRating, value, destinationSplits[1]);
            }
        }
        else
        {
            return new NoConditionRule(destinationSplits[0]);
        }
    }

    private static int ParsePartRating(char letter)
    {
        return letter switch
        {
            'x' => Rating.X,
            'm' => Rating.M,
            'a' => Rating.A,
            's' => Rating.S,
            _ => throw new InvalidOperationException()
        };
    }
}
