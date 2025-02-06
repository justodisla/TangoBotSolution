using TangoBotTrainerApi;

namespace TangoSwingStrategy
{
    public class SwingStrategyAgent : IAgent, ITbotComponent
    {
        public List<IPerceptor> Perceptors { get; set; }
        public List<IActuator> Actuators { get; set; }

        public SwingStrategyAgent() : this("DefaultTradingPlatform", Array.Empty<IPerceptor>(), Array.Empty<IActuator>())
        {
        }

        public SwingStrategyAgent(string tradingPlatformName, IPerceptor[] perceptors, IActuator[] actuators)
        {
            Perceptors = new List<IPerceptor>(perceptors);
            Actuators = new List<IActuator>(actuators);
        }

        public void InjectInputData(object[] input)
        {
            Console.WriteLine("MyAgent received input data.");
        }

        // Implementing the Clone method for shallow copy
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}