using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    public abstract class AbstractAgent : IAgent
    {

        public IEnvInterface EnvInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITestingBooth TestingBooth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void InjectInputData(object[] input)
        {
            Console.WriteLine("MyAgent received input data.");
        }
        // Implementing the Clone method for shallow copy
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        protected List<IActuator> Actuators = new List<IActuator>();
        protected List<IPerceptor> Perceptors = new List<IPerceptor>();

        public List<IPerceptor> GetPercetors()
        {
            return Perceptors;
        }

        public List<IActuator> GetActuators() 
        { 
            return Actuators; 
        }
    }
}
