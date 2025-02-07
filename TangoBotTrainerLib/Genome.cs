using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;

namespace TangoBotTrainerCoreLib
{
    internal class Genome : IGenome
    {
        public int Species { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        internal class Gene : IGenome.IGene
        {
            public int Id { get; set; }

            public int InnovationNumber { get; set; }

            public int ModuleId { get; set; }

            public bool Enabled { get; set; }

            public IGenome ParentGenome { get; set; }

            public IGene Mutate(MutationLevels mutationLevel = MutationLevels.DEFAULT, bool canIgnore = true)
            {
                return GeneticOperator.Mutate(this, mutationLevel);
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public Gene(int id, int innovationNumber, int moduleId, bool enabled, IGenome parentGenome)
            {
                Id = id;
                InnovationNumber = innovationNumber;
                ModuleId = moduleId;
                Enabled = enabled;
                ParentGenome = parentGenome;
            }
        }

        internal class NodeGene : Gene, IGene.INodeGene
        {
            public NodeType Type { get; }
            public int Layer { get; }

            public NodeGene(int id, int innovationNumber, int moduleId, bool enabled, NodeType type, int layer, IGenome genome) : base(id, innovationNumber, moduleId, enabled, genome)
            {
                Type = type;
                Layer = layer;
            }

            public IGene.IConnectionGene[] GetConnections()
            {
                return null;
            }

            public IGene.IConnectionGene[] GetOutGoingConnections()
            {
                throw new NotImplementedException();
            }

            public IGene.IConnectionGene[] GetIncomingConnections()
            {
                throw new NotImplementedException();
            }
        }

        internal class ConnectionGene : Gene, IGene.IConnectionGene
        {
            public string Name { get; }
            public int FromNode { get; set; }
            public int ToNode { get; set; }
            public double Weight { get; set; }


            public ConnectionGene(int id, int innovationNumber, int moduleId, int fromNode, int toNode, double weight, bool enabled, IGenome genome) : base(id, innovationNumber, moduleId, enabled, genome)
            {
                FromNode = fromNode;
                ToNode = toNode;
                Weight = weight;
            }

            public void Reconnect()
            {
                throw new NotImplementedException();
            }
        }

        IGene[] Genes { get; set; }
        public double Fitness { get; set; }
        IGene[] IGenome.Genes { get; set; }

        public Genome(IAgent agent)
        {
        }

        public IGenome Mutate(MutationLevels mutationLevel)
        {
            return GeneticOperator.Mutate(this, mutationLevel);
            //return new Genome();
        }

        public IGenome Crossover(IGenome partner, MutationLevels mutationLevel = MutationLevels.DEFAULT)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns a complete set of species with their population already mutated.
        /// </summary>
        /// <param name="speciesCount"></param>
        /// <param name="siblingsCount"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IGenome[] Speciate(int speciesCount, int siblingsCount = 0)
        {
            Dictionary<int, List<IGenome>> speciesCollection = [];

            for (int i = 0; i < speciesCount; i++)
            {
                speciesCollection.Add(i, new List<IGenome>(this.SpawnSiblingGenome(siblingsCount, MutationLevels.CLOSE_SIBBLINGS)));
            }

            return speciesCollection.SelectMany(x => x.Value).ToArray();
        }

        public IGenome[] SpawnSiblingGenome(int count, MutationLevels mutationLevel)
        {
            List<IGenome> siblings = [];

            bool invalidMutationLevel =
                mutationLevel == MutationLevels.EXTREME ||
                mutationLevel == MutationLevels.INTERSPECIES ||
                mutationLevel == MutationLevels.RANDOM;

            if (invalidMutationLevel)
            {
                throw new NotSupportedException("The mutation level is not appropriate for siblings.");
            }

            for (int i = 0; i < count; i++)
            {
                siblings.Add(this.Mutate(mutationLevel));
            }

            return [.. siblings];
        }

        public IGenome[] SpawnSiblingGenome(int count)
        {
            throw new NotImplementedException();
        }
    }
}
