using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
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

            return new Genome(genome.Agent);
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
            // Clone the current gene to avoid modifying the original
            IGenome.IGene.INodeGene mutatedGene = (IGenome.IGene.INodeGene)seedGene.Clone();

            // Apply bias mutation
            mutatedGene.Bias = MutateBias(mutatedGene.Bias, ResolveMutationLevelValue(mutationLevel).Item1);

            return mutatedGene;
        }

        /// <summary>
        /// Parses the mutation level to a value mutation level and a structural mutation level.
        /// </summary>
        /// <param name="mutationLevel"></param>
        /// <returns></returns>
        public static (double, double) ResolveMutationLevelValue(MutationLevels mutationLevel)
        {
            double mLevel = 0;
            double valueMutationLevel = 0;
            double structuralMutationLevel = 0;

            switch (mutationLevel)
            {
                case MutationLevels.DEFAULT:
                    valueMutationLevel = 1;
                    structuralMutationLevel = 1;
                    break;
                case MutationLevels.CLOSE_SIBLINGS:
                    valueMutationLevel = 0.1;
                    structuralMutationLevel = 0.01;
                    break;
                case MutationLevels.DISTANT_SIBLINGS:
                    valueMutationLevel = 0.3;
                    structuralMutationLevel = 0.25;
                    break;
                case MutationLevels.EXTREME:
                    valueMutationLevel = 0.99;
                    structuralMutationLevel = 0.75;
                    break;
                case MutationLevels.INTERSPECIES:
                    valueMutationLevel = 1;
                    structuralMutationLevel = 0.99;
                    break;
                case MutationLevels.RANDOM:
                    valueMutationLevel = RandomizeHelper.GenerateRandomDouble(0, 1);
                    structuralMutationLevel = RandomizeHelper.GenerateRandomDouble(0, 1);
                    break;
                default:
                    break;
            }
            return (valueMutationLevel, structuralMutationLevel);
        }

        public static StructuralChange SelectStructuralChange()
        {
            int randomValue = RandomizeHelper.GenerateRandomInt(0, 6);

            switch (randomValue)
            {
                case 0:
                    return StructuralChange.ADD_CONNECTION;
                case 1:
                    return StructuralChange.ADD_NODE;
                case 2:
                    return StructuralChange.REMOVE_CONNECTION;
                case 3:
                    return StructuralChange.REMOVE_NODE;
                case 4:
                    return StructuralChange.REMOVE_MODULE;
                case 5:
                    return StructuralChange.ADD_MODULE;
                case 6:
                    return StructuralChange.DO_NOTHING;
                default:
                    break;
            }

            return StructuralChange.ADD_CONNECTION;
        }

        /// <summary>
        /// Mutates the bias of the node.
        /// </summary>
        /// <param name="currentBias">The current bias of the node.</param>
        /// <param name="mutationLevel">Controls the intensity of the mutation.</param>
        /// <returns>The mutated bias.</returns>
        /*
        private static double MutateBias(double currentBias, double mutationLevel)
        {
            if (currentBias > 1 || currentBias < -1)
                throw new Exception("Invalid bias");

            mutationLevel = Math.Abs(mutationLevel);

            double _mutMin = mutationLevel * -1, _mutMax = mutationLevel;

            double lMutationLevel = Math.Abs(RandomizeHelper.GenerateRandomDouble(_mutMin, _mutMax));

            double perturbationLevel = (RandomizeHelper.GenerateRandomDouble(0, 1) * lMutationLevel);

            double biasModifier = currentBias * perturbationLevel;

            bool sumOperation = RandomizeHelper.GenerateRandomBool();

            double modifiedBias = Math.Clamp(sumOperation ? currentBias + biasModifier : currentBias - biasModifier, -1, 1);

            return modifiedBias;
        }*/

        private static double MutateBias(double currentBias, double mutationLevel)
        {
            if (currentBias > 1 || currentBias < -1)
                throw new Exception("Invalid bias");

            mutationLevel = Math.Abs(mutationLevel);

            double perturbationLevel = RandomizeHelper.GenerateRandomDouble(-mutationLevel, mutationLevel);
            double biasModifier = currentBias * perturbationLevel;

            bool sumOperation = RandomizeHelper.GenerateRandomBool();
            double modifiedBias = Math.Clamp(sumOperation ? currentBias + biasModifier : currentBias - biasModifier, -1, 1);

            return modifiedBias;
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
