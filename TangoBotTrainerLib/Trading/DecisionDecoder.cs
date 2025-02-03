public enum Action
{
    Hold,
    Buy,
    Sell
}

public enum OrderType
{
    Market,
    Limit
}

public class TradingDecision
{
    public Action SelectedAction { get; set; }
    public OrderType SelectedOrderType { get; set; }
    public double LimitPrice { get; set; }
    public double TrancheSize { get; set; }

    public static TradingDecision DecodeOutput(double[] outputs)
    {
        int actionIndex = Array.IndexOf(outputs.Take(3).ToArray(), outputs.Take(3).Max());
        Action selectedAction = (Action)actionIndex;

        int orderTypeIndex = Array.IndexOf(outputs.Skip(3).Take(2).ToArray(), outputs.Skip(3).Take(2).Max());
        OrderType selectedOrderType = (OrderType)orderTypeIndex;

        double limitPrice = outputs[5];
        double trancheSize = Math.Clamp(outputs[6], 0, 1);

        return new TradingDecision
        {
            SelectedAction = selectedAction,
            SelectedOrderType = selectedOrderType,
            LimitPrice = limitPrice,
            TrancheSize = trancheSize
        };
    }
}