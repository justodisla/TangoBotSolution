using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;

namespace TangoBotTrainerLib
{
    public class Runtime : IRuntime
    {
        private ITrainingDataComponent _trainingData;

        public ICampaign StartCampaign(IAgent agent, ISupervisor supervisor, ITrainingDataComponent data = null)
        {
            if (data != null)
            {
                SetTrainingData(data);
            }

            var campaign = new Campaign(agent, supervisor, _trainingData);
            campaign.Start();
            return campaign;
        }

        public ICampaign ResumeCampaign(IGenome[] seedGenomes, IAgent agent, int startingCycle = -1)
        {
            var campaign = new Campaign(agent, seedGenomes, startingCycle, _trainingData);
            campaign.Start();
            return campaign;
        }

        public void SetTrainingData(ITrainingDataComponent data)
        {
            _trainingData = data ?? throw new ArgumentNullException(nameof(data), "Training data cannot be null.");
        }
    }
}
