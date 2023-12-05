using Common;
using System.Data;

namespace Day05;

internal class Day05Solver : Solver
{
    private List<long> _seeds = new();

    private Map _seedToSoilMap = new(MapNames.SeedToSoil);
    private Map _soilToFertilizerMap = new(MapNames.SoilToFertilizer);
    private Map _fertilizerToWaterMap = new(MapNames.FertilizerToWater);
    private Map _waterToLightMap = new(MapNames.WaterToLight);
    private Map _lightToTemperatureMap = new(MapNames.LightToTemperature);
    private Map _temperatureToHumidityMap = new(MapNames.TemperatureToHumidity);
    private Map _humidityToLocationMap = new(MapNames.HumidityToLocation);

    public Day05Solver() : base(5, "If You Give A Seed A Fertilizer") { }

    protected override void ParseInput(string[] inputLines)
    {
        _seeds = inputLines[0]
            .Substring(inputLines[0].IndexOf(':') + 1)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        Map? currentMap = null;

        for (var row = 1; row < inputLines.Length; row++)
        {
            var line = inputLines[row];

            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentMap != null)
                {
                    SetMap(currentMap);
                    currentMap = null;
                }
            }
            else if (currentMap == null)
            {
                var name = line.Substring(0, line.IndexOf(':'));
                currentMap = new Map(name);
            }
            else
            {
                var mapping = Mapping.Parse(line);
                currentMap.Mappings.Add(mapping);
            }
        }

        if (currentMap != null)
        {
            SetMap(currentMap);
        }

        void SetMap(Map map)
        {
            switch (map.Name)
            {
                case MapNames.SeedToSoil:
                    _seedToSoilMap = map;
                    break;
                case MapNames.SoilToFertilizer:
                    _soilToFertilizerMap = map;
                    break;
                case MapNames.FertilizerToWater:
                    _fertilizerToWaterMap = map;
                    break;
                case MapNames.WaterToLight:
                    _waterToLightMap = map;
                    break;
                case MapNames.LightToTemperature:
                    _lightToTemperatureMap = map;
                    break;
                case MapNames.TemperatureToHumidity:
                    _temperatureToHumidityMap = map;
                    break;
                case MapNames.HumidityToLocation:
                    _humidityToLocationMap = map;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    protected override string SolvePartOne()
    {
        var answer = _seeds.Select(seedNumber =>
        {
            var soilNumber = _seedToSoilMap.GetCorrespondingNumber(seedNumber);
            var fertilizerNumber = _soilToFertilizerMap.GetCorrespondingNumber(soilNumber);
            var waterNumber = _fertilizerToWaterMap.GetCorrespondingNumber(fertilizerNumber);
            var lightNumber = _waterToLightMap.GetCorrespondingNumber(waterNumber);
            var temperatureNumber = _lightToTemperatureMap.GetCorrespondingNumber(lightNumber);
            var humidityNumber = _temperatureToHumidityMap.GetCorrespondingNumber(temperatureNumber);
            var locationNumber = _humidityToLocationMap.GetCorrespondingNumber(humidityNumber);

            return locationNumber;
        })
        .Min()
        .ToString();

        return answer; // 318728750
    }

    protected override string SolvePartTwo()
    {
        var seedNumberRanges = _seeds
            .Chunk(2)
            .Select(chunk => new NumberRange(chunk[0], chunk[1]))
            .ToList();        

        var soilNumberRanges = MapToCorrespondingNumberRanges(seedNumberRanges, _seedToSoilMap);
        var fertilizerNumberRanges = MapToCorrespondingNumberRanges(soilNumberRanges, _soilToFertilizerMap);
        var waterNumberRanges = MapToCorrespondingNumberRanges(fertilizerNumberRanges, _fertilizerToWaterMap);
        var lightNumberRanges = MapToCorrespondingNumberRanges(waterNumberRanges, _waterToLightMap);
        var temperatureNumberRanges = MapToCorrespondingNumberRanges(lightNumberRanges, _lightToTemperatureMap);
        var humidityNumberRanges = MapToCorrespondingNumberRanges(temperatureNumberRanges, _temperatureToHumidityMap);
        var locationNumberRanges = MapToCorrespondingNumberRanges(humidityNumberRanges, _humidityToLocationMap);

        var answer = locationNumberRanges
            .Min(x => x.Start)
            .ToString();

        return answer; // 37384986
    }

    private static List<NumberRange> MapToCorrespondingNumberRanges(List<NumberRange> numberRanges, Map map)
    {
        var correspondingNumberRanges = new List<NumberRange>();

        foreach (var initialNumberRange in numberRanges)
        {
            var currentNumberRange = initialNumberRange;

            while (currentNumberRange.Length > 0)
            {
                long rangeLimit;
                long correspondingNumberRangeStart;

                var mapping = map.Mappings.Find(x => x.HasMappingFor(currentNumberRange.Start));
                if (mapping != null)
                {
                    rangeLimit = mapping.SourceRangeEnd;
                    correspondingNumberRangeStart = mapping.GetDestination(currentNumberRange.Start);
                }
                else
                {
                    rangeLimit = map.Mappings
                        .Where(x => currentNumberRange.Start < x.SourceRangeStart)
                        .Select(x => x.SourceRangeStart)
                        .DefaultIfEmpty(long.MaxValue)
                        .Min();

                    correspondingNumberRangeStart = currentNumberRange.Start;
                }

                var rangeEnd = long.Min(rangeLimit, currentNumberRange.End);
                var rangeLength = rangeEnd - currentNumberRange.Start + 1;

                var correspondingNumberRange = new NumberRange(correspondingNumberRangeStart, rangeLength);
                correspondingNumberRanges.Add(correspondingNumberRange);

                currentNumberRange = new NumberRange(currentNumberRange.Start + rangeLength, currentNumberRange.Length - rangeLength);
            }
        }

        return correspondingNumberRanges;
    }

    private sealed record NumberRange(long Start, long Length)
    {
        public long End => Start + Length - 1;
    }
}
