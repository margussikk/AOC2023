namespace Day05;

internal class Map
{
    public string Name { get; }
    public List<Mapping> Mappings { get; set; } = new List<Mapping>();

    public Map(string name)
    {
        Name = name;
    }

    public long GetCorrespondingNumber(long sourceNumber)
    {
        var mapping = Mappings.Find(x =>
            x.SourceRangeStart <= sourceNumber &&
            sourceNumber <= x.SourceRangeStart + x.RangeLength - 1);

        if (mapping != null)
        {
            return mapping.DestinationRangeStart + sourceNumber - mapping.SourceRangeStart;
        }

        return sourceNumber;
    }
}
