using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerLib
{
    public class Agent
    {
        public List<string> Inputs = new List<string>();
        public List<string> Outputs = new List<string>();
        public object NeuralNetwork { get; set; }

        public Agent() { 
            
        }
    }
}
