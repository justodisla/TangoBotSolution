using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApp
{
    public class Tester
    {
        public void Process() {

            /*
             Prepare the environment
                Fetch the market data (The strategy should define the symbol the time frame and back data timespan)
                Attach indicators to the market data (The strategy should define the indicators to be used)
                Process the data to create the data cache (Caching the data will speed up the training process, It would be better to cache it in a fast data store)
                Data ready for training
             */

            /*
             Prepare the starting population
                Create the initial genome (This should be a very basic genome with input and output nodes and no connections)
                Mutate the first genome into several very different genomes enough to create different species (this first mutation is for speciation, so it is not a normal mutation but a disruptive one)
                Create the initial generation of species. For each species genome create as many specimens as "InitialSpeciesPopulation" this guaranties that each species starts up with the same number of specimens. Note that this mutaion is a normal mutation
             */

            /*
             Evaluate each specimens
                Each specimen (agent) is created. A neural network is created from the genome and added to the agent.
                Each agent is placed in a "Test Slot" which could be sort of an isolated environment where the data is fed to the agent
            and the output is passed to an instance of the trading platform. The TS manages the data feed throttle, contains an instance of the trading platform dedicated to that single agent and an instance of Supervisor which monitors the work of the agent on the trading platform and is responsible for providing feedback and score to the agent.
            When the evaluation is done, the score is added to the agent which passes it to the genome.
                The fitness is added to the genome
            Once the current cycle is done all genomes have a fitness score.
            The Supervisor has access to the account balance, measures the complexity of the agent's neural network to penalize complexity, whatches for drawdowns, etc. It assigns score using all these factors.
             */

            /*
             Selection of the fittest
             When the cycle ends, for each species the score of each individual is averaged and attributed to the species
            The better performing species retain more of their members and less new members are added to it. For bad performing
            species, more new members are added and less of the old members are kept.
            If the number of species exceeds the maximum number of species, less new members are added until the it dwindles.
            species have a maximum number of members.
            
             */

            /*
             Reproduction
               best performing couples are selected from each species to crossover and mutate
                the new genomes are matched to determine the species they belong to and added to it. If no species is found, a new species is created
                
             */


            //Prepare the environment 


            //Define the agent

            //Configure the populationn

            //Fetch the market data

            //Attach indicators to the market data

            //Process the data to create the data cache

            //Data ready for training




        }

    }
}
