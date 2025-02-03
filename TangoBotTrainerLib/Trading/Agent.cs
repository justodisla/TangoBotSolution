public class Agent
{

    public double LastPrice { get; set; }
    public double BollingerLow { get; set; }
    public double BollingerMean { get; set; }
    public double BollingerHigh { get; set; }
    public double RSI { get; set; }
    public double MACDLine { get; set; }
    public double SignalLine { get; set; }
    public double Volume { get; set; }

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