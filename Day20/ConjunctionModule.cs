namespace Day20;

internal class ConjunctionModule : Module
{
    public Dictionary<string, bool> InputPulses { get; private set; } = new Dictionary<string, bool>();

    public ConjunctionModule(string name, string[] destinationModules) : base(name, destinationModules) { }

    public void InitInputPulses(string[] inputModules)
    {
        InputPulses = inputModules.ToDictionary(x => x, x => false);
    }

    public override List<Signal> ProcessSignal(Signal signal)
    {
        InputPulses[signal.OriginModule] = signal.Pulse;

        var outputPulse = !InputPulses.All(x => x.Value);

        return DestinationModules
            .Select(destinationModule => new Signal(Name, destinationModule, outputPulse))
            .ToList();
    }

    public override void Reset()
    {
        foreach (var module in InputPulses.Keys)
        {
            InputPulses[module] = false;
        }
    }
}
