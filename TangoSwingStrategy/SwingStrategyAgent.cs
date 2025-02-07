using TangoBotTrainerApi;
using TangoBotTrainerCoreLib;

namespace TangoSwingStrategy
{
    public class SwingStrategyAgent : AbstractAgent
    {
        
        private class SwingActuator : IActuator
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public Type Type { get; set; }

            public SwingActuator(string name, string description, Type type)
            {
                Name = name;
                Description = description;
                Type = type;
            }

            public void Actuate()
            {
                throw new System.NotImplementedException();
            }
        }

        private class SwingPerceptor : IPerceptor
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public Type Type { get; set; }
            public SwingPerceptor(string name, string description, Type type)
            {
                Name = name;
                Description = description;
                Type = type;
            }
        }

        public SwingStrategyAgent() : base()
        {
            
            Perceptors.Add(new SwingPerceptor("Close Price", "The asset's closing price", typeof(Double)));
            Perceptors.Add(new SwingPerceptor("Volume", "The asset's period volume", typeof(Double)));

            Actuators.Add(new SwingActuator("Place Market Order", "Determines to place a market order at the Order Price", typeof(Double)));
            Actuators.Add(new SwingActuator("Order Price", "Price at which the order is placed", typeof(Double)));
        }
    }
}