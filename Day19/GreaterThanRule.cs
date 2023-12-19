namespace Day19;

internal class GreaterThanRule : Rule
{
    public int Rating { get; private set; }

    public int Value { get; private set; }
    
    public GreaterThanRule(int rating, int value, string workflowName) : base(workflowName)
    {
        Rating = rating;
        Value = value;
    }

    public override bool Matches(Part part)
    {
        return part.Ratings[Rating] > Value;
    }
}
