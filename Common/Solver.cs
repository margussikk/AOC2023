﻿using System.Diagnostics;

namespace Common;

public abstract class Solver
{
    public int Day { get; }
    public string Name { get; }

    protected Solver(int day, string name)
    {
        Day = day;
        Name = name;
    }

    public void Solve(bool solvePartOne = true, bool solvePartTwo = true)
    {
        Console.WriteLine($"--- Day {Day}: {Name} ---");

        // Parse input
        var parseStopwatch = Stopwatch.StartNew();
        var filename = Path.Combine(Environment.CurrentDirectory, "input.txt");
        var lines = File.ReadAllLines(filename);

        ParseInput(lines);
        parseStopwatch.Stop();

        Console.WriteLine("Parse:");
        PrintTime(parseStopwatch.ElapsedMilliseconds);

        // Solve part one
        if (solvePartOne)
        {
            var stopwatch = Stopwatch.StartNew();
            var answer = SolvePartOne();
            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Part 1:");
            Console.WriteLine(answer);
            PrintTime(stopwatch.ElapsedMilliseconds);
        }

        // Solve part two
        if (solvePartTwo)
        {
            var stopwatch = Stopwatch.StartNew();
            var answer = SolvePartTwo();
            stopwatch.Stop();

            if (solvePartOne)
            {
                Console.WriteLine();
            }

            Console.WriteLine("Part 2:");
            Console.WriteLine(answer);
            PrintTime(stopwatch.ElapsedMilliseconds);
        }
    }

    private static void PrintTime(long elapsedMilliseconds)
    {
        if (elapsedMilliseconds > 1000)
        {
            Console.WriteLine($"Time: {elapsedMilliseconds / 1000} s {elapsedMilliseconds % 1000} ms");
        }
        else
        {
            Console.WriteLine($"Time: {elapsedMilliseconds} ms");
        }
    }

    protected abstract void ParseInput(string[] inputLines);

    protected abstract string SolvePartOne();

    protected abstract string SolvePartTwo();
}
