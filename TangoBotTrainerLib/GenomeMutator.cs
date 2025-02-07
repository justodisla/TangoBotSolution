using System;
using System.Collections.Generic;

namespace TangoBotTrainerApi
{
    public static class GenomeMutator
    {
        /// <summary>
        /// Mutates the given genome based on the specified mutation level.
        /// </summary>
        /// <param name="gnm">The genome to mutate.</param>
        /// <param name="ml">The mutation level to apply.</param>
        /// <returns>A new mutated genome.</returns>
        public static IGenome Mutate(IGenome gnm, IGenome.MutationLevels ml)
        {
            if (gnm == null)
                throw new ArgumentNullException(nameof(gnm), "Genome cannot be null.");

            // Clone the genome to avoid modifying the original
            var mutatedGenome = (IGenome)gnm.Clone();

            // Mutate the genome's genes
            foreach (var gene in mutatedGenome.Genes)
            {
                MutateGene(gene, ml);
            }

            // Apply structural mutations (e.g., add/remove nodes or connections)
            ApplyStructuralMutations(mutatedGenome, ml);

            return mutatedGenome;
        }

        /// <summary>
        /// Mutates a single gene based on the mutation level.
        /// </summary>
        private static void MutateGene(IGenome.IGene gene, IGenome.MutationLevels ml)
        {
            if (gene is IGenome.IGene.IConnectionGene connectionGene)
            {
                // Mutate connection gene
                MutateConnectionGene(connectionGene, ml);
            }
            else if (gene is IGenome.IGene.INodeGene nodeGene)
            {
                // Mutate node gene
                MutateNodeGene(nodeGene, ml);
            }
        }

        /// <summary>
        /// Mutates a connection gene based on the mutation level.
        /// </summary>
        private static void MutateConnectionGene(IGenome.IGene.IConnectionGene connectionGene, IGenome.MutationLevels ml)
        {
            var random = new Random();

            switch (ml)
            {
                case IGenome.MutationLevels.CLOSE_SIBBLINGS:
                    // Small weight perturbation
                    connectionGene.Weight += (random.NextDouble() * 0.1 - 0.05); // ±5% change
                    break;

                case IGenome.MutationLevels.DISTANT_SIBBLINGS:
                    // Moderate weight perturbation
                    connectionGene.Weight += (random.NextDouble() * 0.3 - 0.15); // ±15% change
                    break;

                case IGenome.MutationLevels.INTERSPECIES:
                    // Large weight perturbation
                    connectionGene.Weight += (random.NextDouble() * 0.5 - 0.25); // ±25% change
                    break;

                case IGenome.MutationLevels.EXTREME:
                    // Extreme weight perturbation or complete reset
                    if (random.NextDouble() < 0.5)
                        connectionGene.Weight = random.NextDouble() * 2 - 1; // Reset to random value in [-1, 1]
                    else
                        connectionGene.Weight += (random.NextDouble() * 1.0 - 0.5); // ±50% change
                    break;

                case IGenome.MutationLevels.DEFAULT:
                    // Default mutation: small to moderate perturbation
                    connectionGene.Weight += (random.NextDouble() * 0.2 - 0.1); // ±10% change
                    break;

                case IGenome.MutationLevels.RANDOM:
                    // Completely randomize the weight
                    connectionGene.Weight = random.NextDouble() * 2 - 1; // Random value in [-1, 1]
                    break;
            }

            // Toggle enabled state with a small probability
            if (random.NextDouble() < 0.1) // 10% chance to toggle
            {
                connectionGene.Enabled = !connectionGene.Enabled;
            }
        }

        /// <summary>
        /// Mutates a node gene based on the mutation level.
        /// </summary>
        private static void MutateNodeGene(IGenome.IGene.INodeGene nodeGene, IGenome.MutationLevels ml)
        {
            var random = new Random();

            switch (ml)
            {
                case IGenome.MutationLevels.CLOSE_SIBBLINGS:
                    // Small bias perturbation
                    nodeGene.Bias += (random.NextDouble() * 0.1 - 0.05); // ±5% change
                    break;

                case IGenome.MutationLevels.DISTANT_SIBBLINGS:
                    // Moderate bias perturbation
                    nodeGene.Bias += (random.NextDouble() * 0.3 - 0.15); // ±15% change
                    break;

                case IGenome.MutationLevels.INTERSPECIES:
                    // Large bias perturbation
                    nodeGene.Bias += (random.NextDouble() * 0.5 - 0.25); // ±25% change
                    break;

                case IGenome.MutationLevels.EXTREME:
                    // Extreme bias perturbation or complete reset
                    if (random.NextDouble() < 0.5)
                        nodeGene.Bias = random.NextDouble() * 2 - 1; // Reset to random value in [-1, 1]
                    else
                        nodeGene.Bias += (random.NextDouble() * 1.0 - 0.5); // ±50% change
                    break;

                case IGenome.MutationLevels.DEFAULT:
                    // Default mutation: small to moderate perturbation
                    nodeGene.Bias += (random.NextDouble() * 0.2 - 0.1); // ±10% change
                    break;

                case IGenome.MutationLevels.RANDOM:
                    // Completely randomize the bias
                    nodeGene.Bias = random.NextDouble() * 2 - 1; // Random value in [-1, 1]
                    break;
            }
        }

        /// <summary>
        /// Applies structural mutations to the genome (e.g., adding/removing nodes or connections).
        /// </summary>
        private static void ApplyStructuralMutations(IGenome genome, IGenome.MutationLevels ml)
        {
            var random = new Random();

            // Add a new connection with a certain probability
            if (random.NextDouble() < GetAddConnectionProbability(ml))
            {
                AddRandomConnection(genome);
            }

            // Add a new node with a certain probability
            if (random.NextDouble() < GetAddNodeProbability(ml))
            {
                AddRandomNode(genome);
            }

            // Remove a connection with a certain probability
            if (random.NextDouble() < GetRemoveConnectionProbability(ml))
            {
                RemoveRandomConnection(genome);
            }

            // Remove a node with a certain probability
            if (random.NextDouble() < GetRemoveNodeProbability(ml))
            {
                RemoveRandomNode(genome);
            }
        }

        /// <summary>
        /// Adds a random connection to the genome.
        /// </summary>
        private static void AddRandomConnection(IGenome genome)
        {
            // Logic to add a random connection between two nodes
            // Ensure no duplicate connections and respect graph constraints
        }

        /// <summary>
        /// Adds a random node to the genome.
        /// </summary>
        private static void AddRandomNode(IGenome genome)
        {
            // Logic to add a random hidden node and connect it to existing nodes
        }

        /// <summary>
        /// Removes a random connection from the genome.
        /// </summary>
        private static void RemoveRandomConnection(IGenome genome)
        {
            // Logic to remove a random connection
        }

        /// <summary>
        /// Removes a random node from the genome.
        /// </summary>
        private static void RemoveRandomNode(IGenome genome)
        {
            // Logic to remove a random node and its associated connections
        }

        /// <summary>
        /// Gets the probability of adding a connection based on the mutation level.
        /// </summary>
        private static double GetAddConnectionProbability(IGenome.MutationLevels ml)
        {
            return ml switch
            {
                IGenome.MutationLevels.CLOSE_SIBBLINGS => 0.01,
                IGenome.MutationLevels.DISTANT_SIBBLINGS => 0.05,
                IGenome.MutationLevels.INTERSPECIES => 0.1,
                IGenome.MutationLevels.EXTREME => 0.2,
                IGenome.MutationLevels.DEFAULT => 0.05,
                IGenome.MutationLevels.RANDOM => 0.15,
                _ => 0.0
            };
        }

        /// <summary>
        /// Gets the probability of adding a node based on the mutation level.
        /// </summary>
        private static double GetAddNodeProbability(IGenome.MutationLevels ml)
        {
            return ml switch
            {
                IGenome.MutationLevels.CLOSE_SIBBLINGS => 0.005,
                IGenome.MutationLevels.DISTANT_SIBBLINGS => 0.02,
                IGenome.MutationLevels.INTERSPECIES => 0.05,
                IGenome.MutationLevels.EXTREME => 0.1,
                IGenome.MutationLevels.DEFAULT => 0.02,
                IGenome.MutationLevels.RANDOM => 0.08,
                _ => 0.0
            };
        }

        /// <summary>
        /// Gets the probability of removing a connection based on the mutation level.
        /// </summary>
        private static double GetRemoveConnectionProbability(IGenome.MutationLevels ml)
        {
            return ml switch
            {
                IGenome.MutationLevels.CLOSE_SIBBLINGS => 0.005,
                IGenome.MutationLevels.DISTANT_SIBBLINGS => 0.01,
                IGenome.MutationLevels.INTERSPECIES => 0.02,
                IGenome.MutationLevels.EXTREME => 0.05,
                IGenome.MutationLevels.DEFAULT => 0.01,
                IGenome.MutationLevels.RANDOM => 0.03,
                _ => 0.0
            };
        }

        /// <summary>
        /// Gets the probability of removing a node based on the mutation level.
        /// </summary>
        private static double GetRemoveNodeProbability(IGenome.MutationLevels ml)
        {
            return ml switch
            {
                IGenome.MutationLevels.CLOSE_SIBBLINGS => 0.001,
                IGenome.MutationLevels.DISTANT_SIBBLINGS => 0.005,
                IGenome.MutationLevels.INTERSPECIES => 0.01,
                IGenome.MutationLevels.EXTREME => 0.02,
                IGenome.MutationLevels.DEFAULT => 0.005,
                IGenome.MutationLevels.RANDOM => 0.01,
                _ => 0.0
            };
        }
    }
}