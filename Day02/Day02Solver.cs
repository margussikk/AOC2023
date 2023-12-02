using Common;

namespace Day02;

internal class Day02Solver : Solver
{
    private IReadOnlyList<Game> _games = new List<Game>();

    public Day02Solver() : base(2, "Cube Conundrum") { }

    protected override void ParseInput(string[] inputLines)
    {
        _games = inputLines.Select(Game.Parse).ToList();
    }

    protected override string SolvePartOne()
    {
        var maximumCubeSet = new CubeSet
        {
            Red = 12,
            Green = 13,
            Blue = 14,
        };

        var possibleGames = _games
            .Where(game => game.CubeSets
                .All(cubeSet =>
                    cubeSet.Red <= maximumCubeSet.Red &&
                    cubeSet.Green <= maximumCubeSet.Green &&
                    cubeSet.Blue <= maximumCubeSet.Blue))
            .ToList();

        var answer = possibleGames
            .Select(x => x.Id)
            .Sum()
            .ToString();

        return answer; // 2632
    }

    protected override string SolvePartTwo()
    {
        var answer = _games
            .Select(g => g.Power)
            .Sum()
            .ToString();

        return answer; // 69629
    }
}
