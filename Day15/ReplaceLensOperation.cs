namespace Day15;

internal class ReplaceLensOperation: Operation
{
    public int FocalLength { get; private set; }

    public ReplaceLensOperation(string label, int focalLength) : base(label)
    {
        FocalLength = focalLength;
    }

    public override string ToString()
    {
        return $"{Label}={FocalLength}";
    }
}
