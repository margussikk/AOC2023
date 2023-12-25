namespace Day23;

internal class VertexHiker
{
    public Vertex Vertex { get; }

    public int Distance { get; set; }

    public long VisitedBitMask { get; set; }

    public VertexHiker(Vertex vertex, int distance, long visitedBitMask)
    {
        Vertex = vertex;
        Distance = distance;
        VisitedBitMask = visitedBitMask;
    }
}
