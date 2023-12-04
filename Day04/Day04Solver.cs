using Common;

namespace Day04;

internal class Day04Solver : Solver
{
    private IReadOnlyList<Card> _cards = new List<Card>();

    public Day04Solver() : base(4, "Scratchcards") { }

    protected override void ParseInput(string[] inputLines)
    {
        _cards = inputLines.Select(Card.Parse).ToList();
    }

    protected override string SolvePartOne()
    {
        var answer = _cards
            .Select(c => c.GetMatchesCount())
            .Where(c => c != 0)
            .Select(c => 1 << (c - 1))
            .Sum()
            .ToString();

        return answer; // 15268
    }

    protected override string SolvePartTwo()
    {
        var cardTotals = new int[_cards.Count]; // initialized to 0s
        
        for (var cardIndex = 0; cardIndex < cardTotals.Length; cardIndex++)
        {
            cardTotals[cardIndex]++; // Add the original card

            var matches = _cards[cardIndex].GetMatchesCount();
            for (var copyCardIndex = cardIndex + 1; copyCardIndex < cardIndex + 1 + matches; copyCardIndex++)
            {
                cardTotals[copyCardIndex] += cardTotals[cardIndex];
            }
        }

        var answer = cardTotals
            .Sum()
            .ToString();

        return answer; // 6283755
    }
}
