using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public class MarketState
{
    public double LastPrice { get; set; }
    public double BollingerLow { get; set; }
    public double BollingerMean { get; set; }
    public double BollingerHigh { get; set; }
    public double RSI { get; set; }
    public double MACDLine { get; set; }
    public double SignalLine { get; set; }
    public double Volume { get; set; }

    public MarketState(double lastPrice, double bollingerLow, double bollingerMean, double bollingerHigh, double rSI, double mACDLine, double signalLine, double volume)
    {
        LastPrice = lastPrice;
        BollingerLow = bollingerLow;
        BollingerMean = bollingerMean;
        BollingerHigh = bollingerHigh;
        RSI = rSI;
        MACDLine = mACDLine;
        SignalLine = signalLine;
        Volume = volume;
    }



    public double[] ToNormalizedArray()
    {
        return new double[]
        {
            Normalize(LastPrice, 0, 1000),
            Normalize(BollingerLow, 0, 1000),
            Normalize(BollingerMean, 0, 1000),
            Normalize(BollingerHigh, 0, 1000),
            Normalize(RSI, 0, 100),
            Normalize(MACDLine, -100, 100),
            Normalize(SignalLine, -100, 100),
            Normalize(Volume, 0, 1e6)
        };
    }

    private double Normalize(double value, double min, double max)
    {
        return (value - min) / (max - min);
    }
}

public class DataProcessor
{
    private readonly Indicators _indicators = new Indicators();

    public List<MarketState> ProcessData(JObject rawData, int period)
    {
        // Extract the "Time Series (Daily)" object as a dictionary
        var timeSeries = rawData["Time Series (Daily)"] as JObject;

        if (timeSeries == null)
            throw new ArgumentException("Invalid JSON structure: 'Time Series (Daily)' not found.");

        // Parse closing prices and volumes
        var closePrices = new List<double>();
        var volumes = new List<double>();

        foreach (var entry in timeSeries.Properties())
        {
            var date = entry.Name; // Date is the key
            var values = entry.Value as JObject;

            if (values != null)
            {
                closePrices.Add((double)values["4. close"]);
                volumes.Add((double)values["5. volume"]);
            }
        }

        // Reverse the lists to ensure chronological order (oldest to newest)
        closePrices.Reverse();
        volumes.Reverse();

        // Compute market states
        var marketStates = new List<MarketState>();

        for (int i = period; i < closePrices.Count; i++)
        {
            
            var state = new MarketState(
                lastPrice: closePrices[i],
                bollingerLow: _indicators.CalculateBollingerBands(closePrices.Take(i + 1).ToArray(), period, 2).lowerBand,
                bollingerMean: _indicators.CalculateBollingerBands(closePrices.Take(i + 1).ToArray(), period, 2).middleBand,
                bollingerHigh: _indicators.CalculateBollingerBands(closePrices.Take(i + 1).ToArray(), period, 2).upperBand,
                rSI: _indicators.CalculateRSI(closePrices.Take(i + 1).ToArray(), period),
                mACDLine: _indicators.CalculateMACD(closePrices.Take(i + 1).ToArray(), 12, 26, 9).macdLine,
                signalLine: _indicators.CalculateMACD(closePrices.Take(i + 1).ToArray(), 12, 26, 9).signalLine,
                volume: volumes[i]
                );

            marketStates.Add(state);

            Console.WriteLine($"Last Price: {i}");
        }

        return marketStates;
    }
}