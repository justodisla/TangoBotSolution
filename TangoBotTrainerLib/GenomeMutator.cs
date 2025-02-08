using System;
using System.Collections.Generic;
using static TangoBotTrainerCoreLib.Genome;

namespace TangoBotTrainerApi
{
    public static class GenomeMutator
    {
        private static readonly Random _random = new Random();

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
                MutateConnectionGene(connectionGene, ml);
            }
            else if (gene is IGenome.IGene.INodeGene nodeGene)
            {
                MutateNodeGene(nodeGene, ml);
            }
        }

        /// <summary>
        /// Mutates a connection gene based on the mutation level.
        /// </summary>
        private static void MutateConnectionGene(IGenome.IGene.IConnectionGene connectionGene, IGenome.MutationLevels ml)
        {
            switch (ml)
            {
                case IGenome.MutationLevels.CLOSE_SIBLINGS:
                    // Tiny weight perturbation
                    connectionGene.Weight += (_random.NextDouble() * 0.05 - 0.025); // ±2.5% change
                    break;

                case IGenome.MutationLevels.DISTANT_SIBLINGS:
                    // Moderate weight perturbation, occasional structural changes
                    if (_random.NextDouble() < 0.1) // 10% chance for structural changes
                    {
                        connectionGene.Reconnect();
                    }
                    connectionGene.Weight += (_random.NextDouble() * 0.2 - 0.1); // ±10% change
                    break;

                case IGenome.MutationLevels.INTERSPECIES:
                    // Significant weight changes, structural changes are more common
                    if (_random.NextDouble() < 0.3) // 30% chance for structural changes
                    {
                        connectionGene.Reconnect();
                    }
                    connectionGene.Weight += (_random.NextDouble() * 0.5 - 0.25); // ±25% change
                    break;

                case IGenome.MutationLevels.DEFAULT:
                    // Default mutation: moderate weight perturbation
                    connectionGene.Weight += (_random.NextDouble() * 0.1 - 0.05); // ±5% change
                    break;

                case IGenome.MutationLevels.RANDOM:
                    // Random mutation level between CLOSE_SIBBLINGS and EXTREME
                    var randomLevel = (IGenome.MutationLevels)_random.Next(0, 6); // Random level from 0 to 5
                    MutateConnectionGene(connectionGene, randomLevel);
                    break;

                case IGenome.MutationLevels.EXTREME:
                    // Extreme mutation: large weight changes, frequent structural changes
                    if (_random.NextDouble() < 0.7) // 70% chance for structural changes
                    {
                        connectionGene.Reconnect();
                    }
                    connectionGene.Weight = _random.NextDouble() * 2 - 1; // Reset to random value in [-1, 1]
                    break;
            }

            // Toggle enabled state with a small probability
            if (_random.NextDouble() < 0.1) // 10% chance to toggle
            {
                connectionGene.Enabled = !connectionGene.Enabled;
            }
        }

        /// <summary>
        /// Mutates a node gene based on the mutation level.
        /// </summary>
        private static void MutateNodeGene(IGenome.IGene.INodeGene nodeGene, IGenome.MutationLevels ml)
        {
            switch (ml)
            {
                case IGenome.MutationLevels.CLOSE_SIBLINGS:
                    // Tiny bias perturbation
                    nodeGene.Bias += (_random.NextDouble() * 0.05 - 0.025); // ±2.5% change
                    break;

                case IGenome.MutationLevels.DISTANT_SIBLINGS:
                    // Moderate bias perturbation, occasional structural changes
                    if (_random.NextDouble() < 0.1) // 10% chance for structural changes
                    {
                        AddBiasNodeOrSelfConnection(nodeGene);
                    }
                    nodeGene.Bias += (_random.NextDouble() * 0.2 - 0.1); // ±10% change
                    break;

                case IGenome.MutationLevels.INTERSPECIES:
                    // Significant bias changes, structural changes are more common
                    if (_random.NextDouble() < 0.3) // 30% chance for structural changes
                    {
                        AddBiasNodeOrSelfConnection(nodeGene);
                    }
                    nodeGene.Bias += (_random.NextDouble() * 0.5 - 0.25); // ±25% change
                    break;

                case IGenome.MutationLevels.DEFAULT:
                    // Default mutation: moderate bias perturbation
                    nodeGene.Bias += (_random.NextDouble() * 0.1 - 0.05); // ±5% change
                    break;

                case IGenome.MutationLevels.RANDOM:
                    // Random mutation level between CLOSE_SIBBLINGS and EXTREME
                    var randomLevel = (IGenome.MutationLevels)_random.Next(0, 6); // Random level from 0 to 5
                    MutateNodeGene(nodeGene, randomLevel);
                    break;

                case IGenome.MutationLevels.EXTREME:
                    // Extreme mutation: large bias changes, frequent structural changes
                    if (_random.NextDouble() < 0.7) // 70% chance for structural changes
                    {
                        AddBiasNodeOrSelfConnection(nodeGene);
                    }
                    nodeGene.Bias = _random.NextDouble() * 2 - 1; // Reset to random value in [-1, 1]
                    break;
            }
        }

        /// <summary>
        /// Applies structural mutations to the genome (e.g., adding/removing nodes or connections).
        /// </summary>
        private static void ApplyStructuralMutations(IGenome genome, IGenome.MutationLevels ml)
        {
            switch (ml)
            {
                case IGenome.MutationLevels.CLOSE_SIBLINGS:
                    // Rare structural changes
                    if (_random.NextDouble() < 0.05) // 5% chance
                    {
                        AddRandomConnection(genome);
                    }
                    break;

                case IGenome.MutationLevels.DISTANT_SIBLINGS:
                    // Occasional structural changes
                    if (_random.NextDouble() < 0.1) // 10% chance
                    {
                        AddRandomConnection(genome);
                    }
                    if (_random.NextDouble() < 0.05) // 5% chance
                    {
                        AddRandomNode(genome);
                    }
                    break;

                case IGenome.MutationLevels.INTERSPECIES:
                    // Frequent structural changes
                    if (_random.NextDouble() < 0.3) // 30% chance
                    {
                        AddRandomConnection(genome);
                    }
                    if (_random.NextDouble() < 0.2) // 20% chance
                    {
                        AddRandomNode(genome);
                    }
                    break;

                case IGenome.MutationLevels.DEFAULT:
                    // Moderate structural changes
                    if (_random.NextDouble() < 0.1) // 10% chance
                    {
                        AddRandomConnection(genome);
                    }
                    break;

                case IGenome.MutationLevels.RANDOM:
                    // Random mutation level between CLOSE_SIBBLINGS and EXTREME
                    var randomLevel = (IGenome.MutationLevels)_random.Next(0, 6); // Random level from 0 to 5
                    ApplyStructuralMutations(genome, randomLevel);
                    break;

                case IGenome.MutationLevels.EXTREME:
                    // Aggressive structural changes
                    if (_random.NextDouble() < 0.7) // 70% chance
                    {
                        AddRandomConnection(genome);
                    }
                    if (_random.NextDouble() < 0.5) // 50% chance
                    {
                        AddRandomNode(genome);
                    }
                    break;
            }

            // Remove connections/nodes with small probabilities
            if (_random.NextDouble() < GetRemoveConnectionProbability(ml))
            {
                RemoveRandomConnection(genome);
            }
            if (_random.NextDouble() < GetRemoveNodeProbability(ml))
            {
                RemoveRandomNode(genome);
            }
        }

        /// <summary>
        /// Adds a random connection to the genome.
        /// </summary>
        private static void AddRandomConnection(IGenome genome)
        {
            // Get all nodes in the genome
            var nodes = genome.Genes.OfType<IGenome.IGene.INodeGene>().ToList();

            // Filter nodes based on the current module ID
            int currentModuleId = GetCurrentModuleId(genome); // Assume this method retrieves the current module ID
            //var eligibleNodes = nodes.Where(node => node.ModuleId == currentModuleId).ToList();

            var eligibleFromNodes = nodes.Where(node => node.Type != IGenome.IGene.INodeGene.NodeType.Output).ToList();
            var eligibleToNodes = nodes.Where(node => node.ModuleId == currentModuleId
                && node.Layer >= eligibleFromNodes[0].Layer).ToList();


            if (eligibleFromNodes.Count + eligibleToNodes.Count < 2)
            {
                // Not enough nodes to create a new connection
                return;
            }

            // Randomly select two distinct nodes for the connection
            var fromNode = eligibleFromNodes[_random.Next(eligibleFromNodes.Count)];
            var toNode = eligibleToNodes[_random.Next(eligibleToNodes.Count)];

            // Ensure the connection does not violate layer constraints
            if (!IsValidLayerTransition(fromNode.Layer, toNode.Layer))
            {
                return;
            }

            // Ensure no duplicate connections
            if (IsDuplicateConnection(genome, fromNode.Id, toNode.Id))
            {
                return;
            }

            // Create the new connection gene
            var newConnection = new ConnectionGene
            {
                Id = GenerateUniqueId(genome), // Ensure unique ID for the new connection
                InnovationNumber = GenerateInnovationNumber(), // Assign a unique innovation number
                ModuleId = currentModuleId,
                FromNode = fromNode.Id,
                ToNode = toNode.Id,
                Weight = _random.NextDouble() * 2 - 1, // Random weight in [-1, 1]
                Enabled = true
            };

            // Add the new connection to the genome
            genome.Genes.Add(newConnection);
        }

        /// <summary>
        /// Checks if a layer transition is valid.
        /// </summary>
        private static bool IsValidLayerTransition(int fromLayer, int toLayer)
        {
            // Connections can go:
            // - From one layer to the same layer
            // - From one layer to a layer to the right (larger layer number)
            // - From a node to itself
            return toLayer >= fromLayer;
        }

        /// <summary>
        /// Checks if a connection between two nodes already exists.
        /// </summary>
        private static bool IsDuplicateConnection(IGenome genome, int fromNodeId, int toNodeId)
        {
            return genome.Genes.OfType<IGenome.IGene.IConnectionGene>()
                .Any(conn => conn.FromNode == fromNodeId && conn.ToNode == toNodeId);
        }

        /// <summary>
        /// Generates a unique ID for the new connection.
        /// </summary>
        private static int GenerateUniqueId(IGenome genome)
        {
            // Find the maximum existing ID and increment it
            int maxId = genome.Genes.Any() ? genome.Genes.Max(gene => gene.Id) : 0;
            return maxId + 1;
        }

        /// <summary>
        /// Generates a unique innovation number for the new connection.
        /// </summary>
        private static int GenerateInnovationNumber()
        {
            // In a real implementation, this would be managed globally across genomes
            // For simplicity, we use a random number here
            return _random.Next();
        }

        /// <summary>
        /// Retrieves the current module ID of the genome.
        /// </summary>
        private static int GetCurrentModuleId(IGenome genome)
        {
            // Assume the genome has a property or method to retrieve the current module ID
            // For example, the last added gene might determine the current module ID
            return genome.Genes.Max(gene => gene.ModuleId);
        }

        /// <summary>
        /// Adds a random node to the genome.
        /// </summary>
        private static void AddRandomNode(IGenome genome)
        {
            /*
             Rules:
            A node added must be connected to an existing node to the left.

             */
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
        /// Adds a bias node or self-connection to a node gene.
        /// </summary>
        private static void AddBiasNodeOrSelfConnection(IGenome.IGene.INodeGene nodeGene)
        {
            // Logic to add a bias node or self-connection
        }

        /// <summary>
        /// Gets the probability of removing a connection based on the mutation level.
        /// </summary>
        private static double GetRemoveConnectionProbability(IGenome.MutationLevels ml)
        {
            return ml switch
            {
                IGenome.MutationLevels.CLOSE_SIBLINGS => 0.005,
                IGenome.MutationLevels.DISTANT_SIBLINGS => 0.01,
                IGenome.MutationLevels.INTERSPECIES => 0.05,
                IGenome.MutationLevels.EXTREME => 0.1,
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
                IGenome.MutationLevels.CLOSE_SIBLINGS => 0.001,
                IGenome.MutationLevels.DISTANT_SIBLINGS => 0.005,
                IGenome.MutationLevels.INTERSPECIES => 0.02,
                IGenome.MutationLevels.EXTREME => 0.05,
                IGenome.MutationLevels.DEFAULT => 0.005,
                IGenome.MutationLevels.RANDOM => 0.01,
                _ => 0.0
            };
        }
    }
}