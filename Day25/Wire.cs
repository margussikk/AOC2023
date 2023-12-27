namespace Day25;

internal class Wire
{
    public Component Component1 { get; }

    public Component Component2 { get; }

    public Wire(Component component1, Component component2)
    {
        Component1 = component1;
        Component2 = component2;
    }

    public override string ToString()
    {
        return $"{Component1.Name} - {Component2.Name}";
    }
}
