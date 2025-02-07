using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public class Runtime : IRuntime
    {
        private readonly Campaign _campaign;

        public Runtime()
        {
            _campaign = new Campaign();
            _campaign.Start();

            //List<IGenome> seedGenomes = [new Genome()];
            //_campaign.StartSeeded([.. seedGenomes]);
        }
    }
}