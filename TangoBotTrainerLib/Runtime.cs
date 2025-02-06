using TangoBotTrainerApi;
using TangoBotTrainerCoreLib;

public class Runtime : IRuntime
{
    private readonly DependencyContainer _container;

    public Runtime(DependencyContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public ICampaign StartCampaign(IAgent agent, ISupervisor supervisor, ITrainingDataComponent data = null)
    {
        if (data != null)
        {
            SetTrainingData(data);
        }

        var campaign = new Campaign(agent, supervisor, _container.Resolve<ITrainingDataComponent>());
        campaign.Start();
        return campaign;
    }

    public ICampaign ResumeCampaign(IGenome[] seedGenomes, IAgent agent, int startingCycle = -1)
    {
        var campaign = new Campaign(agent, seedGenomes, startingCycle, _container.Resolve<ITrainingDataComponent>());
        campaign.Start();
        return campaign;
    }

    public void SetTrainingData(ITrainingDataComponent data)
    {
        _container.Register(data);
    }
}