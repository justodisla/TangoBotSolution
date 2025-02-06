using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;

namespace TangoSwingStrategy
{
    public class SwingMarketData : ITrainingDataComponent, ITbotComponent
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            Console.WriteLine("SwingMarketData initialized.");
        }
    }
}
