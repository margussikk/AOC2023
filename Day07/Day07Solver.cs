using Common;

namespace Day07;

internal class Day07Solver : Solver
{
    private List<Hand> _hands = new List<Hand>();

    public Day07Solver() : base(7, "Camel Cards") { }

    protected override void ParseInput(string[] inputLines)
    {
        _hands = inputLines.Select(Hand.Parse).ToList();
    }

    protected override string SolvePartOne()
    {
        _hands.ForEach(hand => hand.FillHandTypeForPartOne());       

        var sortedHands = _hands.ToList();
        sortedHands.Sort(new HandComparer("23456789TJQKA"));

        var answer = sortedHands
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum()
            .ToString();

        return answer; // 247961593
    }

    protected override string SolvePartTwo()
    {
        _hands.ForEach(hand => hand.FillHandTypeForPartTwo());

        var sortedHands = _hands.ToList();
        sortedHands.Sort(new HandComparer("J23456789TQKA"));

        var answer = sortedHands
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum()
            .ToString();

        return answer; // 248750699
    }
}
