using System;
using System.Linq;

public class Indicators
{
    /// <summary>
    /// Calculates the Simple Moving Average (SMA) over a specified period.
    /// </summary>
    /// <param name="closePrices">Array of closing prices.</param>
    /// <param name="period">The number of periods to consider for the SMA.</param>
    /// <returns>The SMA value.</returns>
    public double CalculateSMA(double[] closePrices, int period)
    {
        if (closePrices.Length < period)
            throw new ArgumentException("Not enough data points for the specified period.");

        double sum = 0;
        for (int i = closePrices.Length - period; i < closePrices.Length; i++)
        {
            sum += closePrices[i];
        }

        return sum / period;
    }

    /// <summary>
    /// Calculates Bollinger Bands (Lower, Middle, Upper) over a specified period.
    /// </summary>
    /// <param name="closePrices">Array of closing prices.</param>
    /// <param name="period">The number of periods to consider for the Bollinger Bands.</param>
    /// <param name="deviations">The number of standard deviations for the bands.</param>
    /// <returns>A tuple containing the Lower Band, Middle Band, and Upper Band.</returns>
    public (double lowerBand, double middleBand, double upperBand) CalculateBollingerBands(double[] closePrices, int period, double deviations)
    {
        if (closePrices.Length < period)
            throw new ArgumentException("Not enough data points for the specified period.");

        // Calculate the middle band (SMA)
        double middleBand = CalculateSMA(closePrices, period);

        // Calculate the standard deviation
        double mean = middleBand;
        double variance = 0;
        for (int i = closePrices.Length - period; i < closePrices.Length; i++)
        {
            variance += Math.Pow(closePrices[i] - mean, 2);
        }
        double stdDev = Math.Sqrt(variance / period);

        // Calculate the upper and lower bands
        double upperBand = middleBand + (stdDev * deviations);
        double lowerBand = middleBand - (stdDev * deviations);

        return (lowerBand, middleBand, upperBand);
    }

    /// <summary>
    /// Calculates the Relative Strength Index (RSI) over a specified period.
    /// </summary>
    /// <param name="closePrices">Array of closing prices.</param>
    /// <param name="period">The number of periods to consider for the RSI.</param>
    /// <returns>The RSI value.</returns>
    public double CalculateRSI(double[] closePrices, int period)
    {
        if (closePrices.Length < period + 1)
            throw new ArgumentException("Not enough data points for the specified period.");

        double gainSum = 0;
        double lossSum = 0;

        // Calculate initial gains and losses
        for (int i = 1; i <= period; i++)
        {
            double change = closePrices[closePrices.Length - i] - closePrices[closePrices.Length - i - 1];
            if (change > 0)
                gainSum += change;
            else
                lossSum += Math.Abs(change);
        }

        double avgGain = gainSum / period;
        double avgLoss = lossSum / period;

        // Smooth the averages over the remaining periods
        for (int i = period + 1; i < closePrices.Length; i++)
        {
            double change = closePrices[closePrices.Length - i] - closePrices[closePrices.Length - i - 1];
            double gain = change > 0 ? change : 0;
            double loss = change < 0 ? Math.Abs(change) : 0;

            avgGain = (avgGain * (period - 1) + gain) / period;
            avgLoss = (avgLoss * (period - 1) + loss) / period;
        }

        double rs = avgLoss == 0 ? 100 : avgGain / avgLoss;
        return 100 - (100 / (1 + rs));
    }

    /// <summary>
    /// Calculates the Moving Average Convergence Divergence (MACD).
    /// </summary>
    /// <param name="closePrices">Array of closing prices.</param>
    /// <param name="fastPeriod">The fast EMA period (e.g., 12).</param>
    /// <param name="slowPeriod">The slow EMA period (e.g., 26).</param>
    /// <param name="signalPeriod">The signal line period (e.g., 9).</param>
    /// <returns>A tuple containing the MACD Line and Signal Line.</returns>
    public (double macdLine, double signalLine) CalculateMACD(double[] closePrices, int fastPeriod, int slowPeriod, int signalPeriod)
    {
        if (closePrices.Length < slowPeriod)
            //throw new ArgumentException("Not enough data points for the specified periods.");
        return (0, 0);

        // Calculate the fast and slow EMAs
        double fastEma = CalculateEMA(closePrices, fastPeriod);
        double slowEma = CalculateEMA(closePrices, slowPeriod);

        // Calculate the MACD line
        double macdLine = fastEma - slowEma;

        // Calculate the signal line (EMA of the MACD line)
        double[] macdLineValues = new double[closePrices.Length];
        for (int i = 0; i < closePrices.Length; i++)
        {
            macdLineValues[i] = CalculateEMA(closePrices.Take(i + 1).ToArray(), fastPeriod) -
                                CalculateEMA(closePrices.Take(i + 1).ToArray(), slowPeriod);
        }
        double signalLine = CalculateEMA(macdLineValues, signalPeriod);

        return (macdLine, signalLine);
    }

    /// <summary>
    /// Helper method to calculate the Exponential Moving Average (EMA).
    /// </summary>
    /// <param name="closePrices">Array of closing prices.</param>
    /// <param name="period">The number of periods to consider for the EMA.</param>
    /// <returns>The EMA value.</returns>
    private double CalculateEMA(double[] closePrices, int period)
    {
        if (closePrices.Length < period)
            //throw new ArgumentException("Not enough data points for the specified period.");
        return 0;

        double smoothingFactor = 2.0 / (period + 1);
        double ema = closePrices[0];

        for (int i = 1; i < closePrices.Length; i++)
        {
            ema = (closePrices[i] - ema) * smoothingFactor + ema;
        }

        return ema;
    }
}