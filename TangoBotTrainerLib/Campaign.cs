using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public class Campaign : ICampaign
    {
        private readonly IAgent _agent;
        private readonly ISupervisor _supervisor;
        private readonly ITrainingDataComponent _trainingData;
        private IGenome[] seedGenomes;
        private int startingCycle;
        private ITrainingDataComponent trainingDataComponent;

        private List<IGenome> GenomePool { get; set; }

        public Campaign()
        {
            _agent = DependencyInjection.Resolve<IAgent>();
            _supervisor = DependencyInjection.Resolve<ISupervisor>();
            _trainingData = DependencyInjection.Resolve<ITrainingDataComponent>();
        }
        public void Start()
        {
            Console.WriteLine("Campaign started.");
            PrepareTrainingData();
            InitializeGenomePool();
            Evolve();
            Cleanup();
        }

        private void InitializeGenomePool()
        {
            IGenome adamGenome = new Genome();
        }

        private void PrepareTrainingData()
        {
            _trainingData.Initialize();
        }

        private void Evolve()
        {
            Console.WriteLine("Evolution completed.");
        }

        private void Cleanup()
        {
            Console.WriteLine("Resources cleaned up.");
        }

        public void SetStartingCycle(int cycle)
        {
            throw new NotImplementedException();
        }

        public INeuralNetwork GetNeuralNetwork()
        {
            throw new NotImplementedException();
        }
    }
}