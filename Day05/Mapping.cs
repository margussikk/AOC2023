namespace Day05;

internal class Mapping
{
    public long SourceRangeStart { get; private set; }

    public long DestinationRangeStart { get; private set; }

    public long RangeLength { get; private set; }

    public long SourceRangeEnd => SourceRangeStart + RangeLength - 1;

    public bool HasMappingFor(long number) => SourceRangeStart <= number && number <= SourceRangeStart + RangeLength - 1;

    public long GetDestination(long number) => DestinationRangeStart + number - SourceRangeStart;

    public static Mapping Parse(string input)
    {
        var splits = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var mapping = new Mapping
        {
            DestinationRangeStart = long.Parse(splits[0]),
            SourceRangeStart = long.Parse(splits[1]),
            RangeLength = long.Parse(splits[2])
        };

        return mapping;
    }
}
