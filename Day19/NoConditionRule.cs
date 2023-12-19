namespace Day19;

internal class NoConditionRule : Rule
{
    public NoConditionRule(string workflowName) : base(workflowName) { }

    public override bool Matches(Part part)
    {
        return true;
    }
}
