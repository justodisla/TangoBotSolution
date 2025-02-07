using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApi
{
    public interface IGenome
    {
        public enum MutationLevels
        {
            CLOSE_SIBBLINGS,
            DISTANT_SIBBLINGS,
            INTERSPECIES,
            EXTREME,
            DEFAULT,
            RANDOM
        }

        public interface IGene : ICloneable
        {
            /// <summary>
            /// The id of the gene.
            /// </summary>
            int Id { get; }

            /// <summary>
            /// The innovation number of the gene.
            /// </summary>
            int InnovationNumber { get; }
            
            /// <summary>
            /// The module id of the gene.
            /// </summary>
            int ModuleId { get; }

            /// <summary>
            /// Whether the gene is enabled or not.
            /// </summary>
            bool Enabled { get; set; }

            /// <summary>
            /// Mutate the gene to create a new gene.
            /// </summary>
            /// <param name="mutationLevel"></param>
            IGene Mutate(MutationLevels mutationLevel = MutationLevels.DEFAULT, bool canIgnore = true);

            public interface INodeGene : IGene
            {
                public enum NodeType
                {
                    Input,
                    Output,
                    Hidden,
                    Bias
                }
                public NodeType Type { get; }

                public int Layer { get; }

                /// <summary>
                /// Returns the connections that are connected to this node.
                /// </summary>
                /// <returns></returns>
                IGene.IConnectionGene[] GetConnections();

                /// <summary>
                /// Returns the outgoing connections that are connected to this node.
                /// </summary>
                /// <returns></returns>
                IGene.IConnectionGene[] GetOutGoingConnections();

                /// <summary>
                /// Returns the incoming connections that are connected to this node.
                /// </summary>
                /// <returns></returns>
                IGene.IConnectionGene[] GetIncomingConnections();

            }

            public interface IConnectionGene : IGene
            {
                public int FromNode { get; }
                public int ToNode { get; }
                public double Weight { get; set; }

                /// <summary>
                /// Connects an unconnected connection or
                /// reconnects to another from node or to node or both.
                /// </summary>
                void Reconnect();

            }
        }

        public interface ISpecies { 
            int Number { get; }
            IGenome[] Members { get; }
        }

        double Fitness { get; set; }

        int Species { get; set; }

        IGene[] Genes { get; set; }

        /// <summary>
        /// Mutate the genome to create a new genome.
        /// </summary>
        /// <param name="mutationLevel"></param>
        /// <returns></returns>
        public IGenome Mutate(MutationLevels mutationLevel);

        /// <summary>
        /// Crossover with another genome to create a new genome.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public IGenome Crossover(IGenome partner, MutationLevels mutationLevel = 0);

        /// <summary>
        /// Create a number of species from the current genome making sure that new Genomes are of different species.
        /// </summary>
        /// <param name="speciesCount"></param>
        /// <returns></returns>
        public IGenome[] Speciate(int speciesCount, int siblingsCount = 0);

        /// <summary>
        /// Create a number of sibblings from the current genome.
        /// Sibblings are mutated versions of the current genome but remain in the same species.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IGenome[] SpawnSiblingGenome(int count);


    }
}
