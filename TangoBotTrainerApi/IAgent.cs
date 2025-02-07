using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApi
{
    public interface IAgent : ITbotComponent
    {
        //List<IPerceptor> Perceptors { get; }
        //List<IActuator> Actuators { get; }

        IEnvInterface EnvInterface { get; set; }

        ITestingBooth TestingBooth { get; set; }

        List<IPerceptor> GetPercetors();

        List<IActuator> GetActuators();
    }
}
