namespace Day23;

internal class Vertex
{
    public long BitMask { get; }

    public Tile Tile { get; }

    public List<Edge> Edges { get; } = new List<Edge>();

    public Vertex(int index, Tile tile)
    {
        BitMask = 1L << index;
        Tile = tile;
    }
}
