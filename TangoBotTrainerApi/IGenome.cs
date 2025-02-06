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
        public interface IGene
        {   
            public int Id { get; }

            public int InnovationNumber { get; }

            public int ModuleId { get; }

            public bool Enabled { get; }

            void Mutate(int mutationLevel);

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
                public double Weight { get; }

                /// <summary>
                /// Connects an unconnected connection or
                /// reconnects to another from node or to node or both.
                /// </summary>
                void Reconnect();
                
            }
        }

        /// <summary>
        /// Mutate the genome to create a new genome.
        /// </summary>
        /// <param name="mutationLevel"></param>
        /// <returns></returns>
        public IGenome Mutate(int mutationLevel);

        /// <summary>
        /// Crossover with another genome to create a new genome.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public IGenome Crossover(IGenome partner);

        /// <summary>
        /// Set the fitness of the genome.
        /// </summary>
        /// <param name="fitness"></param>
        public void SetFitness(double fitness);
        public double GetFitness();

        /// <summary>
        /// Get all the genes in the genome.
        /// </summary>
        /// <returns></returns>
        public IGene[] GetGenes();

        /// <summary>
        /// Create a number of species from the current genome making sure that new Genomes are of different species.
        /// </summary>
        /// <param name="speciesCount"></param>
        /// <returns></returns>
        public IGenome[] Speciate(int speciesCount);

        /// <summary>
        /// Create a number of sibblings from the current genome.
        /// Sibblings are mutated versions of the current genome but remain in the same species.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IGenome[] SpawnSibblingGenome(int count);
    }
}
