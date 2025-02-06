using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    internal class TestingBooth : ITestingBooth
    {
        private object _agent;
        private object _envInterface;
        private ISupervisor _supervisor;

        public TestingBooth(IAgent agent, IEnvInterface envInterface, ISupervisor supervisor) { 
            _agent = agent.Clone();
            _envInterface = envInterface.Clone();
            _supervisor = supervisor;
        }
    }
}
