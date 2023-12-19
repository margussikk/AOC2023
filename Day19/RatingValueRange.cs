namespace Day19;

internal class RatingValueRange
{
    public long Min { get; set; }
    public long Max { get; set; }

    public long Length => Max - Min + 1;

    public RatingValueRange(long min, long max)
    {
        Min = min;
        Max = max;
    }
}
