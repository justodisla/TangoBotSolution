using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome;
using static TangoBotTrainerCoreLib.Genome;

namespace TangoBotTrainerCoreLib
{
    internal static class GeneticOperator
    {
        private static Random _random = new Random();

        public static IGenome[] SortGenomesByFitness(IGenome[] genomes, bool highOnTop = true)
        {
            return genomes;
        }

        public static IGenome Crossover(IGenome parent1, IGenome parent2)
        {
            throw new NotImplementedException();
        }

        public static IGenome Mutate(IGenome genome, MutationLevels mutationLevel)
        {

            //IGene[] genes = genome.GetGenes();

            return new Genome();
        }

        internal static IGenome.IGene Mutate(Gene gene, MutationLevels mutationLevel)
        {
            if (gene is IGenome.IGene.IConnectionGene connectionGene)
            {
                // The gene is an IConnectionGene, and you can now work with it as such
                Console.WriteLine("The gene is an IConnectionGene.");

                return MutateConnectionGene(connectionGene, mutationLevel, false);

                // Perform mutation specific to IConnectionGene
                //return MutateConnectionGene(connectionGene, mutationLevel);
            }
            else if (gene is IGenome.IGene.INodeGene nodeGene)
            {
                // Handle other types of genes (e.g., INodeGene)
                Console.WriteLine("The gene is not an IConnectionGene.");

                return MutateNodeGene(nodeGene, mutationLevel);
            }
            else 
            {
                // Handle other types of genes (e.g., INodeGene)
                Console.WriteLine("The gene is any other thing.");
                throw new NotSupportedException("Mutation for this gene type is not implemented.");
            }
        }

        internal static IGenome.IGene MutateNodeGene(IGenome.IGene.INodeGene seedGene, MutationLevels mutationLevel)
        {
            /*
            // Clone the current gene to avoid modifying the original
            IGenome.IGene.INodeGene mutatedGene = new NodeGene();
            // Change the type of the node with a small probability
            if (_random.NextDouble() < 0.1) // 10% chance to change type
            {
                mutatedGene.Type = (NodeType)_random.Next(0, Enum.GetValues(typeof(NodeType)).Length);
            }
            return mutatedGene;
            */

            return seedGene;
        }

        internal static IGenome.IGene MutateConnectionGene(IGenome.IGene.IConnectionGene seedGene, MutationLevels mutationLevel, bool canIgnore)
        {

            /*
            // Clone the current gene to avoid modifying the original
            IGenome.IGene.IConnectionGene mutatedGene = new ConnectionGene();

            // Apply weight mutation
            mutatedGene.Weight = MutateWeight(mutatedGene.Weight, mutationLevel);

            // Toggle enabled/disabled state with a small probability
            if (_random.NextDouble() < 0.1) // 10% chance to toggle
            {
                mutatedGene.Enabled = !mutatedGene.Enabled;
            }

            // Reconnect the connection with a small probability
            if (!canIgnore && _random.NextDouble() < 0.05) // 5% chance to reconnect
            {
                mutatedGene.Reconnect();
            }

            return mutatedGene;
            */

            return seedGene;
        }

        /// <summary>
        /// Mutates the weight of the connection.
        /// </summary>
        /// <param name="currentWeight">The current weight of the connection.</param>
        /// <param name="mutationLevel">Controls the intensity of the mutation.</param>
        /// <returns>The mutated weight.</returns>
        private static double MutateWeight(double currentWeight, int mutationLevel)
        {
            double perturbationRange = mutationLevel > 0 ? 1.0 / mutationLevel : 0.1; // Smaller mutationLevel -> larger perturbation

            if (_random.NextDouble() < 0.9) // 90% chance to perturb
            {
                return currentWeight + (_random.NextDouble() * 2 - 1) * perturbationRange; // Add random perturbation
            }
            else // 10% chance to reset
            {
                return _random.NextDouble() * 2 - 1; // Reset to a new random value in [-1, 1]
            }
        }
    }
}
