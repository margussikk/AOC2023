namespace Day23;

internal class Edge
{
    public Vertex StartVertex { get; }

    public Vertex EndVertex { get; }

    public int Distance { get; }

    public Edge(Vertex startVertex, Vertex endVertex, int distance)
    {
        StartVertex = startVertex;
        EndVertex = endVertex;
        Distance = distance;
    }
}
