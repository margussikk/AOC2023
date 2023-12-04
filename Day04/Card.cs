namespace Day04;

internal class Card
{
    public int Id { get; private set; }

    public IReadOnlyList<int> WinningNumbers { get; private set; } = new List<int>();

    public IReadOnlyList<int> NumbersYouHave { get; private set; } = new List<int>();

    public int GetMatchesCount()
    {
        return WinningNumbers.Intersect(NumbersYouHave).Count();
    }

    public static Card Parse(string input)
    {
        var card = new Card();

        var splits = input.Split(':');

        card.Id = int.Parse(splits[0].Replace("Card", ""));

        splits = splits[1].Split('|');

        card.WinningNumbers = splits[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        card.NumbersYouHave = splits[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        return card;
    }
}
