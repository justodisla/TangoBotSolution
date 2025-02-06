using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public class Runtime : IRuntime
    {
        public Runtime()
        {
            //ITrainingDataComponent tps = DependencyInjection.Resolve<ITrainingDataComponent>();
            ISupervisor supervisor = DependencyInjection.Resolve<ISupervisor>();
        }

        public void StartCampaign()
        {

            IAgent agent = DependencyInjection.Resolve<IAgent>();
            ISupervisor supervisor = DependencyInjection.Resolve<ISupervisor>();
            ITrainingDataComponent data = DependencyInjection.Resolve<ITrainingDataComponent>();

            StartCampaign(agent, supervisor, data);

        }

        public ICampaign StartCampaign(IAgent agent, ISupervisor supervisor, ITrainingDataComponent data = null)
        {
            var campaign = new Campaign(agent, supervisor, DependencyInjection.Resolve<ITrainingDataComponent>());
            campaign.Start();
            return campaign;
        }

        public ICampaign ResumeCampaign(IGenome[] seedGenomes, IAgent agent, int startingCycle = -1)
        {
            var campaign = new Campaign(agent, seedGenomes, startingCycle, DependencyInjection.Resolve<ITrainingDataComponent>());
            campaign.Start();
            return campaign;
        }

        public void SetTrainingData(ITrainingDataComponent data)
        {
            //DependencyInjection.Register(data);
        }
    }
}