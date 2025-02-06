using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApi
{
    public interface IRuntime
    {
        ICampaign StartCampaign(IAgent agent, ISupervisor supervisor, ITrainingDataComponent data = null);
        ICampaign ResumeCampaign(IGenome[] seedGenomes, IAgent agent, int startingCycle = -1);
        void SetTrainingData(ITrainingDataComponent data);
    }
}
