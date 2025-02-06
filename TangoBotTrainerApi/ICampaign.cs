using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApi
{
    public interface ICampaign
    {
        void Start();
        void SetStartingCycle(int cycle);
        INeuralNetwork GetResult();
    }
}
