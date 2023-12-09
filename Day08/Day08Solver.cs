using Common;
using System.Text.RegularExpressions;

namespace Day08;

// Input is designed such that LCM is the correct answer and not generalizable for other inputs.
// Since instructions list loops, nodes lists must also loops for LCM to work.
// Nodes form loops and the cycle length is the same as the steps to get from xxA to the xxZ.
// xxA node is outside of the loop.
// Loops don't intersect and every loop has only one xxZ.
// xxA -> ... -> ... -> XXX -> ... -> ... -> xxZ -> ... -> XXX

internal partial class Day08Solver : Solver
{
    private List<Instruction> _instructions = new List<Instruction>();
    private List<Node> _nodes = new List<Node>();

    public Day08Solver() : base(8, "Haunted Wasteland") { }

    protected override void ParseInput(string[] inputLines)
    {
        _instructions = inputLines[0]
            .Select(ParseInstruction)
            .ToList();

        var knownNodes = new Dictionary<string, Node>();
        var nodeDependenciesDict = new Dictionary<string, List<NodeDependency>>();

        for (var i = 2; i < inputLines.Length; i++)
        {
            var matches = InputLineRegex().Matches(inputLines[i]);
            if (matches.Count == 1)
            {
                List<NodeDependency>? nodeDependencies;

                // Current node
                var currentNodeName = matches[0].Groups[1].Value;
                var currentNode = new Node(currentNodeName);

                knownNodes[currentNode.Name] = currentNode;

                // Left node
                var leftNodeName = matches[0].Groups[2].Value;
                if (knownNodes.TryGetValue(leftNodeName, out var leftNode))
                {
                    currentNode.SetLeft(leftNode);
                }
                else
                {
                    if (!nodeDependenciesDict.TryGetValue(leftNodeName, out nodeDependencies))
                    {
                        nodeDependencies = new List<NodeDependency>();
                        nodeDependenciesDict[leftNodeName] = nodeDependencies;
                    }

                    nodeDependencies.Add(new NodeDependency(currentNode, Instruction.Left));
                }

                // Right node
                var rightNodeName = matches[0].Groups[3].Value;
                if (knownNodes.TryGetValue(rightNodeName, out var rightNode))
                {
                    currentNode.SetRight(rightNode);
                }
                else
                {
                    if (!nodeDependenciesDict.TryGetValue(rightNodeName, out nodeDependencies))
                    {
                        nodeDependencies = new List<NodeDependency>();
                        nodeDependenciesDict[rightNodeName] = nodeDependencies;
                    }

                    nodeDependencies.Add(new NodeDependency(currentNode, Instruction.Right));
                }

                // Deal with dependants
                if (nodeDependenciesDict.TryGetValue(currentNode.Name, out nodeDependencies))
                {
                    foreach (var nodeDepenendency in nodeDependencies)
                    {
                        if (nodeDepenendency.Instruction == Instruction.Left)
                        {
                            nodeDepenendency.Node.SetLeft(currentNode);
                        }
                        else
                        {
                            nodeDepenendency.Node.SetRight(currentNode);
                        }
                    }

                    nodeDependenciesDict.Remove(currentNode.Name);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        if (nodeDependenciesDict.Any())
        {
            throw new InvalidOperationException("Something didn't get linked");
        }

        _nodes = knownNodes.Values.ToList();
    }

    protected override string SolvePartOne()
    {
        var node = _nodes.Single(x => x.Name == "AAA");

        var answer = GetStepsToEnd(node, x => x!.Name != "ZZZ");

        return answer.ToString(); // 18727
    }

    protected override string SolvePartTwo()
    {
        var stepsList = _nodes
            .Where(x => x.Name[2] == 'A')
            .Select(x => GetStepsToEnd(x, n => n!.Name[2] != 'Z'))
            .ToList();

        var answer = MathUtils.LeastCommonMultiple(stepsList);

        return answer.ToString(); // 18024643846273
    }

    private long GetStepsToEnd(Node? node, Func<Node?, bool> criteria)
    {
        var steps = 0L;
        var index = 0;

        while (criteria(node))
        {
            node = _instructions[index] == Instruction.Left ? node!.Left : node!.Right;

            steps++;
            index = (index + 1) % _instructions.Count;
        }

        return steps;
    }

    private static Instruction ParseInstruction(char letter)
    {
        return letter switch
        {
            'L' => Instruction.Left,
            'R' => Instruction.Right,
            _ => throw new InvalidOperationException()
        };
    }

    [GeneratedRegex("([0-9A-Z]{3}) = \\(([0-9A-Z]{3}), ([0-9A-Z]{3})\\)")]
    private static partial Regex InputLineRegex();

    private sealed record NodeDependency(Node Node, Instruction Instruction);
}
