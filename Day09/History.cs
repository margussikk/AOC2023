namespace Day09;

internal class History
{
    public List<List<long>> Sequences { get; private set; } = new List<List<long>>();

    public static History Parse(string input)
    {
        var sequences = new List<List<long>>
        {
            new List<long>(input
                .Split(' ')
                .Select(long.Parse))
        };

        while (sequences[^1].Exists(x => x != 0))
        {
            var nextSequence = Enumerable
                .Range(0, sequences[^1].Count - 1)
                .Select(x => sequences[^1][x + 1] - sequences[^1][x])
                .ToList();

            sequences.Add(nextSequence);
        }

        var history = new History
        {
            Sequences = sequences
        };

        return history;
    }
}
