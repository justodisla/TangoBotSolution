using TangoBotTrainerApi;

namespace TangoSwingStrategy
{
    public class Supervisor : ISupervisor, ITbotComponent
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public double Evaluate()
        {
            Console.WriteLine("Supervisor evaluated performance.");
            return 0.85; // Example score
        }
    }
}