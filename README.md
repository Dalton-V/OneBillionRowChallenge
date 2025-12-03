# OneBillionRowChallenge
This repository contains the code and resources for the One Billion Row Challenge, 
a project aimed at processing and analyzing large datasets efficiently.

## Overview

This is project holds documentation and code samples related to handling and processing large datasets,
with a focus on performance optimization and scalability. Documenting each iteration and recording the improvements found.

## Initial Approach - POC

The inital approach involved loading the entire file into memory, processing each line into a dictionary to calculate average temperatures per location.

```csharp
using System.Diagnostics;
using System.Text;

Console.WriteLine("Starting.");
var sw = Stopwatch.StartNew();

var fileLines = File.ReadAllLines(@"E:\Code\TestData\measurements.txt");

var locationData = new Dictionary<string, TemperatureRecord>();
foreach (var line in fileLines)
{
    // 0 = Location, 1 = Temperature 
    var data = line.Split(';');

    if (locationData.TryGetValue(data[0], out var record))
    {
        record.Count++;
        record.Sum += double.Parse(data[1]);
        record.Min = Math.Min(record.Min, double.Parse(data[1]));
        record.Max = Math.Max(record.Max, double.Parse(data[1]));
    }
    else
    {
        locationData[data[0]] = new TemperatureRecord
        {
            Count = 1,
            Sum = double.Parse(data[1]),
            Min = double.Parse(data[1]),
            Max = double.Parse(data[1])
        };
    }
}

var sb = new StringBuilder();

foreach (var kvp in locationData)
{
    sb.Append($"{kvp.Key}={kvp.Value.Min}/{kvp.Value.Sum/kvp.Value.Count}/{kvp.Value.Min}, ");
}

Console.WriteLine(sb.ToString());
sw.Stop();

Console.WriteLine("Finished.");
Console.WriteLine($"Elapsed: {sw.Elapsed}");

internal class TemperatureRecord
{
    public int Count { get; set; }
    public double Sum { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}
```

This approach was straightforward but had significant performance issues due to high memory consumption and processing time.

{:test}
