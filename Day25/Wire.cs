namespace Day25;

internal class Wire
{
    public int Id { get; }

    public Component Component1 { get; }

    public Component Component2 { get; }

    public Wire(int id, Component component1, Component component2)
    {
        Id = id;
        Component1 = component1;
        Component2 = component2;
    }

    public override string ToString()
    {
        return $"{Component1.Name} - {Component2.Name}";
    }
}
