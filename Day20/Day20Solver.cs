using Common;

namespace Day20;

internal class Day20Solver : Solver
{
    private Dictionary<string, Module> _modules = new Dictionary<string, Module>();

    public Day20Solver() : base(20, "Pulse Propagation") { }

    protected override void ParseInput(string[] inputLines)
    {
        _modules = inputLines.Select(Module.Parse).ToDictionary(x => x.Name);

        var conjunctionModules = _modules
            .Where(x => x.Value is ConjunctionModule)
            .Select(x => x.Value)
            .Cast<ConjunctionModule>()
            .ToList();

        foreach (var conjunctionModule in conjunctionModules)
        {
            var inputModules = _modules
                .Where(x => x.Value.DestinationModules.Contains(conjunctionModule.Name))
                .Select(x => x.Value.Name)
                .ToArray();

            conjunctionModule.InitInputPulses(inputModules);
        }
    }

    protected override string SolvePartOne()
    {
        var highPulses = 0L;
        var lowPulses = 0L;

        var queue = new Queue<Signal>();

        for (var i = 0; i < 1000; i++)
        {
            queue.Enqueue(new Signal("button", "broadcaster", false));

            while (queue.Count > 0)
            {
                var signal = queue.Dequeue();
                if (signal.Pulse)
                {
                    highPulses++;
                }
                else
                {
                    lowPulses++;
                }

                if (_modules.TryGetValue(signal.DestinationModule, out var module))
                {
                    var newSignals = module.ProcessSignal(signal);
                    foreach (var newSignal in newSignals)
                    {
                        queue.Enqueue(newSignal);
                    }
                }
            }
        }

        var answer = highPulses * lowPulses;

        return answer.ToString(); // 731517480
    }

    protected override string SolvePartTwo()
    {
        foreach (var kvp in _modules)
        {
            kvp.Value.Reset();
        }

        // Only one conjunction module sends signals to 'rx'. This module has multiple inputs and all of those need to be high.
        var feederModule = (ConjunctionModule) _modules.Values.First(x => x.DestinationModules.Contains("rx"));

        var counters = new Dictionary<string, long>();

        var queue = new Queue<Signal>();

        var buttonPresses = 0;

        while(counters.Count < feederModule.InputPulses.Count)
        {
            buttonPresses++;

            queue.Enqueue(new Signal("button", "broadcaster", false));

            while (queue.Count > 0)
            {
                var signal = queue.Dequeue();

                if (signal.DestinationModule == feederModule.Name && signal.Pulse && !counters.ContainsKey(signal.OriginModule))
                {
                    counters[signal.OriginModule] = buttonPresses;
                }

                if (_modules.TryGetValue(signal.DestinationModule, out var module))
                {
                    var newSignals = module.ProcessSignal(signal);
                    foreach (var newSignal in newSignals)
                    {
                        queue.Enqueue(newSignal);
                    }
                }
            }
        }

        var answer = MathUtils.LeastCommonMultiple(counters.Values);

        return answer.ToString(); // 244178746156661
    }
}