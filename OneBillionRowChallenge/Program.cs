using OneBillionRowChallenge;
using System.Diagnostics;
using System.Text;

var createTestFiles = new CreateTestFiles();
createTestFiles.CreateFile(100_000_000);

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
    sb.Append($"{kvp.Key}={kvp.Value.Min}/{Math.Round(kvp.Value.Sum/kvp.Value.Count, 1)}/{kvp.Value.Max}, ");
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