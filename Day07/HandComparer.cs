namespace Day07;

internal class HandComparer : IComparer<Hand>
{
    private readonly Dictionary<char, int> _cardStrengths;

    public HandComparer(string cardSequence)
    {
        _cardStrengths = cardSequence
            .Select((x, index) => new
            {
                Card = x,
                Strength = index
            })
            .ToDictionary(x => x.Card, x => x.Strength);
    }

    public int Compare(Hand? first, Hand? second)
    {
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first));
        }

        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        if (first.HandType < second.HandType)
        {
            return -1;
        }
        else if (first.HandType > second.HandType)
        {
            return 1;
        }
        else
        {
            for (var i = 0; i < first.Cards.Length; i++)
            {
                var firstCardStrength = _cardStrengths[first.Cards[i]];
                var secondCardStrength = _cardStrengths[second.Cards[i]];

                if (firstCardStrength < secondCardStrength)
                {
                    return -1;
                }
                else if (firstCardStrength > secondCardStrength)
                {
                    return 1;
                }
            }
        }

        return 0;
    }
}
