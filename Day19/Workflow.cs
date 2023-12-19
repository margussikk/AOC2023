namespace Day19;

internal class Workflow
{
    public string Name { get; private set; } = string.Empty;
    public List<Rule> Rules { get; private set; } = new List<Rule>();

    public string GetNextWorkflowName(Part part)
    {
        return Rules.First(x => x.Matches(part)).WorkflowName;
    }

    public static Workflow Parse(string input)
    {
        var rulesStartIndex = input.IndexOf('{');

        var workFlow = new Workflow
        {
            Name = input.Substring(0, rulesStartIndex),
            Rules = input.Substring(rulesStartIndex)
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Split(',')
                .Select(Rule.Parse)
                .ToList()
        };

        return workFlow;
    }
}
