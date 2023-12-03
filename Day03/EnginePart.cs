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
            var topAdjacentRow = Row - 1;
            var bottomAdjacentRow = Row + 1;
            var leftAdjacentColumn = Column - 1;
            var rightAdjacentColumn = Column + (Value.Length - 1) + 1;

            var otherRightColumn = other.Column + (other.Value.Length - 1);

            return other.Row >= topAdjacentRow && // Top
                   other.Row <= bottomAdjacentRow && // Bottom
                   ((other.Column >= leftAdjacentColumn && other.Column <= rightAdjacentColumn) || // Start: Left and Right
                    (otherRightColumn >= leftAdjacentColumn && otherRightColumn <= rightAdjacentColumn)); // End: Left and Right

        }
    }
}
