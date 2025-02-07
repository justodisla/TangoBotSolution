using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public class Campaign : ICampaign
    {
        private readonly IAgent _agent;
        private readonly ISupervisor _supervisor;
        private readonly ITrainingDataComponent _trainingData;
        
        private int _startingCycle;
        private ITrainingDataComponent _trainingDataComponent;
        private IGenomePool _genomePool;

        IGenomePool ICampaign.GenomePool => _genomePool;

        public Campaign()
        {
            _agent = DependencyInjection.Resolve<IAgent>();
            _supervisor = DependencyInjection.Resolve<ISupervisor>();
            _trainingData = DependencyInjection.Resolve<ITrainingDataComponent>();
        }

        public void StartSeeded(IGenome[] seedGenomes = null, int startCycle = -1)
        {
            _genomePool = new GenomePool(seedGenomes);
            Start(startCycle);
        }

        public void Start(int startCycle = -1)
        {
            if(startCycle != 0)
            {
                SetStartingCycle(startCycle);
            }

            //Capture the needed data and caches it
            PrepareTrainingData();

            //The goal is to populate the Genome Pool with the initial population.
            InitializeGenomePool();

            //Evolve the population until a champion neural network is found.
            Evolve(_startingCycle);

            //Clean up resources and saves information for further optimization.
            Cleanup();
        }

        private void InitializeGenomePool()
        {
            if(_genomePool == null || _genomePool.Pool.Count == 0)
            {
                Genome basicGenome = new Genome();
                _genomePool = new GenomePool();
                this._genomePool.Pool = new List<IGenome>(basicGenome.Speciate(5, 5));
            }
        }

        private void PrepareTrainingData()
        {
            _trainingData.Initialize();
        }

        private void Evolve(int startingCycle)
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

        /*
        public void Start(IGenome[] seedGenomes = null, int startCycle = -1)
        {
            throw new NotImplementedException();
        }

        */
    }
}