public class FitnessEvaluator
{
    public double Evaluate(Genome genome, List<MarketState> marketStates)
    {
        double portfolioValue = 10000; // Initial capital

        foreach (var state in marketStates)
        {
            var outputs = genome.Evaluate(state.ToNormalizedArray());
            var decision = TradingDecision.DecodeOutput(outputs);

            portfolioValue += ExecuteTrade(decision, state.LastPrice);
        }

        return portfolioValue; // Higher portfolio value = better fitness
    }

    private double ExecuteTrade(TradingDecision decision, double currentPrice)
    {
        // Logic to execute trades and calculate profit/loss
        return 0; // Placeholder
    }
}