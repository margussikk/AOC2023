namespace Day08;

internal class Node
{
    public string Name { get; set; }
    public Node? Left { get; private set; }
    public Node? Right { get; private set; }

    public Node(string name)
    {
        Name = name;
    }
    public void SetLeft (Node left)
    {
        Left = left;
    }
    public void SetRight(Node right)
    {
        Right = right;
    }
}
