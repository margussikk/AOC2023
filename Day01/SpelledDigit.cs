namespace Day01;

internal sealed class SpelledDigit
{
    public string Word { get; }
    public int Length { get; }
    public char Digit { get; }

    public SpelledDigit(int value, string word)
    {
        Word = word;
        Length = word.Length;
        Digit = Convert.ToChar('0' + value);
    }
}
