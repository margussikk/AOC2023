namespace Day19;

internal class WorkflowRatingValueRange
{
    public string WorkflowName { get; set; } = "";

    public RatingValueRange[] RatingValueRanges { get; set; } = new RatingValueRange[4];


    public WorkflowRatingValueRange Clone()
    {
        return new WorkflowRatingValueRange
        {
            WorkflowName = WorkflowName,
            RatingValueRanges = RatingValueRanges.Select(x => new RatingValueRange(x.Min, x.Max)).ToArray(),
        };
    }
}
