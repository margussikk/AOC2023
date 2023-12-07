namespace Day07;

internal class Hand
{
    public char[] Cards { get; private set; } = Array.Empty<char>();
    
    public long Bid { get; private set; }

    public HandType HandType { get; private set; }

    public void FillHandTypeForPartOne()
    {
        var countsList = Cards
            .ToLookup(x => x)
            .Select(x => x.Count())
            .Order()
            .ToList();

        HandType = GetHandType(countsList);
    }

    public void FillHandTypeForPartTwo()
    {
        var countsList = Cards
            .Where(x => x != 'J')
            .ToLookup(x => x)
            .Select(x => x.Count())
            .Order()
            .ToList();

        var jokerCount = 5 - countsList.Sum();
        if (jokerCount == 5)
        {
            HandType = HandType.FiveOfAKind;
        }
        else
        {
            // Add jokers to the highest cards group (high card, pair, three of a kind, four of a kind)
            countsList[^1] += jokerCount;

            HandType = GetHandType(countsList);
        }
    }

    public static Hand Parse(string input)
    {
        var splits = input.Split(' ');

        return new Hand
        {
            Cards = splits[0].ToArray(),
            Bid = long.Parse(splits[1]),
            HandType = HandType.HighCard
        };
    }

    private static HandType GetHandType(List<int> countsList)
    {
        return countsList switch
        {
            [5] => HandType.FiveOfAKind,
            [1, 4] => HandType.FourOfAKind,
            [2, 3] => HandType.FullHouse,
            [1, 1, 3] => HandType.ThreeOfAKind,
            [1, 2, 2] => HandType.TwoPair,
            [1, 1, 1, 2] => HandType.OnePair,
            _ => HandType.HighCard
        };
    }
}
