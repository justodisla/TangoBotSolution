using System.Collections.Generic;
using System.Linq;

namespace TangoBot.IndicatorsLib
{
    public class SmaCalculator
    {
        public decimal CalculateSMA(List<decimal> prices, int period)
        {
            if (prices.Count < period)
                return 0; // or handle insufficient data case

            return prices.TakeLast(period).Average();
        }
    }
}
