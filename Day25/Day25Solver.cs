using Common;
using System.Text;

namespace Day25;

internal class Day25Solver : Solver
{
    private readonly List<Component> _components = new List<Component>();
    private readonly List<Wire> _wires = new List<Wire>();

    public Day25Solver() : base(25, "Snowverload") { }

    protected override void ParseInput(string[] inputLines)
    {
        var wireId = 0;
        var components = new Dictionary<string, Component>();
        
        for (var i = 0; i < inputLines.Length; i++)
        {
            var splits = inputLines[i].Split(':');

            var mainComponentName = splits[0].Trim();
            var otherComponentNames = splits[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

            if(!components.TryGetValue(mainComponentName, out var mainComponent))
            {
                mainComponent = new Component(mainComponentName);
                components.Add(mainComponent.Name, mainComponent);
            }

            foreach (var otherComponentName in otherComponentNames)
            {
                if (!components.TryGetValue(otherComponentName, out var otherComponent))
                {
                    otherComponent = new Component(otherComponentName);
                    components.Add(otherComponent.Name, otherComponent);
                }

                // Sanity check
                if (mainComponent.Wires.Exists(x => (x.Component1.Name == mainComponent.Name && x.Component2.Name == otherComponent.Name) ||
                    (x.Component1.Name == otherComponent.Name && x.Component2.Name == mainComponent.Name)))
                {
                    throw new InvalidOperationException($"Wire already exists between {mainComponent.Name} and {otherComponent.Name}");
                }

                wireId++;
                var wire = new Wire(wireId, mainComponent, otherComponent);

                mainComponent.AddWire(wire);
                otherComponent.AddWire(wire);

                _wires.Add(wire);
            }
        }

        _components.AddRange(components.Values);
    }

    protected override string SolvePartOne()
    {
        var numberOfWiresToCut = 3;
        var wiresToCut = new List<Wire>();

        int answer = 0;

        var notEnoughWires = _components.Exists(x => x.Wires.Count <= numberOfWiresToCut);
        if (notEnoughWires)
        {
            throw new InvalidOperationException($"Solution requires all components to have more than 3 wires. There needs to be at least 1 extra path exiting the start component but not reaching the end component.");
        }

        // Find all the paths from the first component to all the other components. Keep track of wires used and don't use them on the next run.
        // Eventually two components are in different groups and these groups are connected by 3 wires. Once these 3 wires are used up, we have reached the end.
        // Since all the components have to have at least one more wire than needs to be cut, visited set contains all the components in start components group,
        // end component is in another group. Size of the first group is same as size of the visited set, second group size is total - first group size.
        var startComponent = _components[0];
        foreach (var endComponent in _components.Skip(1))
        {
            var found = false;
            var visited = new HashSet<Component>();
            var usedWires = new HashSet<Wire>();
            var pathsFound = 0;

            // Find multiple paths between the same start and end component, but don't use the same wire twice. Eventually there are no available wires left.
            // If components are in different groups, then there are only 3 paths, components in the same group have at least 4 paths between them.
            do
            {
                found = false;
                visited.Clear();

                var queue = new Queue<QueueItem>();

                var queueItem = new QueueItem(startComponent, new List<Wire>());
                queue.Enqueue(queueItem);

                while (queue.Count > 0)
                {
                    queueItem = queue.Dequeue();

                    if (queueItem.Component == endComponent)
                    {
                        found = true;
                        break;
                    }

                    if (visited.Contains(queueItem.Component))
                    {
                        continue;
                    }

                    visited.Add(queueItem.Component);

                    foreach (var wire in queueItem.Component.Wires.Where(w => !usedWires.Contains(w)))
                    {
                        var nextComponent = wire.Component1 == queueItem.Component ? wire.Component2 : wire.Component1;

                        var newWires = new List<Wire>(queueItem.Wires)
                        {
                            wire
                        };

                        var nextQueueItem = new QueueItem(nextComponent, newWires);
                        queue.Enqueue(nextQueueItem);
                    }
                }

                // Mark wires as used so not use those on the next runs
                if (found)
                {
                    pathsFound++;

                    foreach (var wire in queueItem.Wires)
                    {
                        usedWires.Add(wire);
                    }
                }

            } while (found);

#pragma warning disable S2583 // Conditionally executed code should be reachable
            if (pathsFound == numberOfWiresToCut)
            {
                var groupComponents = visited.ToList();
                wiresToCut = _wires // Find wires which connect two groups
                    .Where(x => (groupComponents.Contains(x.Component1) && !groupComponents.Contains(x.Component2)) ||
                                (groupComponents.Contains(x.Component2) && !groupComponents.Contains(x.Component1)))
                    .ToList();

                answer = visited.Count * (_components.Count - visited.Count);
                break;
            }
#pragma warning restore S2583 // Conditionally executed code should be reachable
        }

        var stringBuilder = new StringBuilder();

        var wireNames = string.Join(", ", wiresToCut.Select(w => $"{w.Component1.Name}-{w.Component2.Name}"));
        stringBuilder.AppendLine($"Wires to cut: {wireNames}"); // rcn-xkf, dht-xmv, cms-thk
        stringBuilder.AppendLine(answer.ToString()); // 543036

        return stringBuilder.ToString();
    }

    protected override string SolvePartTwo()
    {
        return "TODO";
    }

    private sealed record QueueItem(Component Component, List<Wire> Wires);
}