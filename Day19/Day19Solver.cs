using Common;

namespace Day19;

internal class Day19Solver : Solver
{
    private readonly Dictionary<string, Workflow> _workflows = new Dictionary<string, Workflow>();
    private readonly List<Part> _parts = new List<Part>();

    public Day19Solver() : base(19, "Aplenty") { }

    protected override void ParseInput(string[] inputLines)
    {
        int lineIndex;

        // Workflows
        for (lineIndex = 0; lineIndex < inputLines.Length && !string.IsNullOrWhiteSpace(inputLines[lineIndex]); lineIndex++)
        {
            var workflow = Workflow.Parse(inputLines[lineIndex]);
            _workflows.Add(workflow.Name, workflow);
        }
        
        lineIndex++; // Empty line

        // Parts
        for (; lineIndex < inputLines.Length && !string.IsNullOrWhiteSpace(inputLines[lineIndex]); lineIndex++)
        {
            var part = Part.Parse(inputLines[lineIndex]);
            _parts.Add(part);
        }
    }

    protected override string SolvePartOne()
    {
        var answer = 0;

        foreach (var part in _parts)
        {
            var workflowName = WorkflowName.In;
            while (workflowName != WorkflowName.Accepted && workflowName != WorkflowName.Rejected)
            {
                workflowName = _workflows[workflowName].GetNextWorkflowName(part);
            }

            if (workflowName == WorkflowName.Accepted)
            {
                answer += part.Ratings.Sum();
            }
        }
        
        return answer.ToString(); // 432434
    }

    protected override string SolvePartTwo()
    {
        var answer = 0L;

        var queue = new Queue<WorkflowRatingValueRange>();

        var currentWorkFlowRatingValueRange = new WorkflowRatingValueRange
        {
            WorkflowName = WorkflowName.In,
            RatingValueRanges =
            [
                new RatingValueRange(1, 4000), // X
                new RatingValueRange(1, 4000), // M
                new RatingValueRange(1, 4000), // A
                new RatingValueRange(1, 4000), // S
            ]
        };

        queue.Enqueue(currentWorkFlowRatingValueRange);

        while (queue.Count > 0)
        {
            currentWorkFlowRatingValueRange = queue.Dequeue();

            var workFlow = _workflows[currentWorkFlowRatingValueRange.WorkflowName];

            foreach (var rule in workFlow.Rules)
            {
                var nextWorkflowRatingValueRange = currentWorkFlowRatingValueRange.Clone();
                nextWorkflowRatingValueRange.WorkflowName = rule.WorkflowName;

                if (rule is LessThanRule lessThanRule)
                {
                    var min = currentWorkFlowRatingValueRange.RatingValueRanges[lessThanRule.Rating].Min;
                    var max = lessThanRule.Value - 1;

                    nextWorkflowRatingValueRange.RatingValueRanges[lessThanRule.Rating] = new RatingValueRange(min, max);
                    currentWorkFlowRatingValueRange.RatingValueRanges[lessThanRule.Rating].Min = lessThanRule.Value;
                }
                else if (rule is GreaterThanRule greaterThanRule)
                {
                    var min = greaterThanRule.Value + 1;
                    var max = currentWorkFlowRatingValueRange.RatingValueRanges[greaterThanRule.Rating].Max;

                    nextWorkflowRatingValueRange.RatingValueRanges[greaterThanRule.Rating] = new RatingValueRange(min, max);
                    currentWorkFlowRatingValueRange.RatingValueRanges[greaterThanRule.Rating].Max = greaterThanRule.Value;
                }
                else if (rule is NoConditionRule)
                {
                    // Do nothing
                }

                if (nextWorkflowRatingValueRange.WorkflowName == WorkflowName.Accepted)
                {
                    answer += nextWorkflowRatingValueRange.RatingValueRanges[Rating.X].Length *
                              nextWorkflowRatingValueRange.RatingValueRanges[Rating.M].Length *
                              nextWorkflowRatingValueRange.RatingValueRanges[Rating.A].Length *
                              nextWorkflowRatingValueRange.RatingValueRanges[Rating.S].Length;
                }
                else if (nextWorkflowRatingValueRange.WorkflowName != WorkflowName.Rejected)
                {
                    queue.Enqueue(nextWorkflowRatingValueRange);
                }
            }
        }

        return answer.ToString(); // 132557544578569
    }
}