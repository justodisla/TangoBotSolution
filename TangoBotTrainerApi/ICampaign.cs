using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApi
{
    public interface ICampaign
    {
        /// <summary>
        /// Get the genomes that have been or are being evolved in this campaign.
        /// </summary>
        /// <returns></returns>
        IGenome[] GetGenomes();

        /// <summary>
        /// Start the Campaign from the beginning.
        /// </summary>
        void Start();

        /// <summary>
        /// Instructs the Campaign to start evolving the supplied seed genomes from the specified cycle.
        /// In no cycle is specified, the Campaign will start from the beginning.
        /// </summary>
        /// <param name="seedGenomes"></param>
        /// <param name="startCycle"></param>
        void Start(IGenome[] seedGenomes = null, int startCycle = -1);

        /// <summary>
        /// Returns the evolved Neural Network.
        /// If the evolution hast not yet completed, this method will return the best performing Neural Network so far.
        /// </summary>
        /// <returns></returns>
        INeuralNetwork GetNeuralNetwork();


    }
}
