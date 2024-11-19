using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Indicator
{
    /// <summary>
    /// Implementer is an indicator that receives inputs and renders
    /// output. For instance an SMA has one input (period) and one output (value)
    /// In some instance this SMA can have also a CalculateFrom input that is used to 
    /// determine if the average will be calculated from the close, open, high or low.
    /// </summary>
    public interface IIndicator
    {
        /// <summary>
        /// Represents the input of the indicator.
        /// </summary>
        public class Input
        {
            public string InputName { get; set; }
            public Type InputType { get; set; }
            public object InputValue { get; set; }
        }
        /// <summary>
        /// Represents the output of the indicator.
        /// </summary>
        public class Output
        {
            public string OutputName { get; set; }
            public Type OutputType { get; set; }
            public object OutputValue { get; set; }
        }

        /// <summary>
        /// Holds the inputs of the indicator that will be used to provide
        /// parameters to the indicator to perform the calculation.
        /// </summary>
        public List<Input> Inputs { get; set; }

        /// <summary>
        /// Holds the outputs of the indicator
        /// </summary>
        public List<Output> Outputs { get; set; }

        /// <summary>
        /// Performs the calculation of the indicator. for every datapoint
        /// </summary>
        void Calculate();
    }
}
