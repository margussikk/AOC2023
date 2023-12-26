namespace Day25;

internal class Component
{
    public string Name { get; }

    public List<Wire> Wires { get; } = new List<Wire>();

    public Component(string name)
    {
        Name = name;
    }

    public void AddWire(Wire wire)
    {
        Wires.Add(wire);
    }
}
