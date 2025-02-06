using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public class Runtime : IRuntime
    {
        ICampaign _campaign;

        public Runtime()
        {
            _campaign = new Campaign();
            //_campaign.Start();

            List<IGenome> seedGenomes = new List<IGenome>();
            seedGenomes.Add(new Genome());
            _campaign.StartSeeded(seedGenomes.ToArray());
        }
    }
}