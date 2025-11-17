using System.Diagnostics;

namespace OneBillionRowChallenge;

internal class CreateTestFiles
{
    public void CreateFile(int numberOfRows)
    {
        var weatherStationNames = GetWeatherStationNames();
        var estimatedFileSize = EstimateFileSize(weatherStationNames, numberOfRows);
        Console.WriteLine(estimatedFileSize);
        BuildTestData(weatherStationNames, numberOfRows);
    }

    private void BuildTestData(string[] weatherStationNames, int numRowsToCreate)
    {
        var fs = File.Create(@"E:\Code\TestData\measurements.txt");
        fs.Close();

        var sw = Stopwatch.StartNew();
        var cooldestTemp = -99.99;
        var hottestTemp = 99.99;

        var currentRow = 0;
        var r = new Random();
        while (currentRow < numRowsToCreate)
        {
            var fileLines = new List<string>();
            for(var i = 0; i < 10000; i++)
            {
                var station = weatherStationNames[r.Next(0, weatherStationNames.Length)];
                var temperature = Math.Round(cooldestTemp + (r.NextDouble() * (hottestTemp - cooldestTemp)), 1);
                fileLines.Add($"{station};{temperature}");
                currentRow++;
                if (currentRow >= numRowsToCreate)
                    break;
            }
            File.AppendAllLines(@"E:\Code\TestData\measurements.txt", fileLines);
        }

    }

    private string[] GetWeatherStationNames()
    {
        var weatherStationData = File.ReadAllLines(@"E:\Code\TestData\weather_stations.csv");

        var weatherStationNames = new string[weatherStationData.Length - 2];
        for (int i = 0; i < weatherStationData.Length; i++)
        {
            string? line = weatherStationData[i];
            if (line == null)
                continue;

            if (line.Contains('#'))
                continue;

            var splits = line.Split(';');
            weatherStationNames[i - 2] = splits[0];
        }

        return weatherStationNames;
    }

    private string EstimateFileSize(string[] weatherStationNames, int numberOfRows)
    {
        var totalNameBytes = 0.0;

        foreach (var name in weatherStationNames)
            totalNameBytes += System.Text.Encoding.UTF8.GetByteCount(name);

        var averageNameBytes = totalNameBytes / weatherStationNames.Length;
        var humanReadableBytes = GetHumanReadableBytes(numberOfRows * averageNameBytes);

        return $"Estimated max file size is: {humanReadableBytes}";
    }

    private string GetHumanReadableBytes(double bytes)
    {
        var supportedConversions = new string[] { "bytes", "KiB", "MiB", "GiB" };

        for (var i = 0; i < supportedConversions.Length; i++)
        {
            if (bytes < 1024.0)
                return $"{Math.Round(bytes, 1)} {supportedConversions[i]}";

            bytes /= 1024.0;
        }

        return $"{bytes} bytes is larger than the supported conversions.";
    }
}
