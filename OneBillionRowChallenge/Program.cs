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
    }
    else
    {
        locationData[data[0]] = new TemperatureRecord
        {
            Count = 1,
            Sum = double.Parse(data[1])
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