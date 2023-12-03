namespace Day03
{
    internal class EnginePart
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public string Value { get; set; } = ".";

        public bool IsNumberPart => char.IsDigit(Value, 0);

        public bool IsSymbolPart => !IsNumberPart && Value != ".";

        public bool IsGearPart => Value == "*";

        public bool IsAdjacentTo(EnginePart other)
        {
            return other.Row >= (Row - 1) && // Top
                   other.Row <= (Row + 1) && // Bottom
                   other.Column >= (Column - 1) && // Left
                   other.Column <= (Column + (Value.Length - 1) + 1); // Right
        }
    }
}
