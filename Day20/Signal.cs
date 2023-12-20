namespace Day20;

internal class Signal
{
    public string OriginModule { get; }
    public string DestinationModule { get; }
    public bool Pulse { get; }

    public Signal(string originModule, string destinationModule, bool pulse)
    {
        OriginModule = originModule;
        DestinationModule = destinationModule;
        Pulse = pulse;
    }
}
