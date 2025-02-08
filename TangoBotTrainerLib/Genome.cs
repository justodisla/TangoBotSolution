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
    internal partial class Genome : IGenome
    {
        public int Species { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<IGenome.IGene> Genes { get; set; }
        public double Fitness { get; set; }

        public IAgent Agent { get; set; }

        public int CurrentInnovationNumber { get; set; }

        private int GetNextInnovationNumber()
        {
            CurrentInnovationNumber++;
            return CurrentInnovationNumber;
        }
        
        public Genome(IAgent agent)
        {
            Genes ??= [];

            //Create the input nodes
            if (agent == null)
            {
                throw new ArgumentNullException();
            }

            Agent = agent;

            //List<object> sx = [agent.GetPercetors(), agent.GetActuators()];

            foreach (IPerceptor p in agent.GetPercetors())
            {
                //NodeType nt = p.GetType() == typeof(IPerceptor) ? NodeType.Input : NodeType.Output;
                IGenome.IGene.INodeGene n = new NodeGene(1, -1, -1, true, NodeType.Input, -1, this);
                Genes.Add(n);
            }

            foreach (IActuator p in agent.GetActuators())
            {
                //NodeType nt = p.GetType() == typeof(IPerceptor) ? NodeType.Input : NodeType.Output;
                IGenome.IGene.INodeGene n = new NodeGene(1, -1, -1, true, NodeType.Output, -1, this);
                Genes.Add(n);
            }
        }

        public IGenome Mutate(MutationLevels mutationLevel)
        {
            return GenomeMutator.Mutate(this, mutationLevel);
            ///return GeneticOperator.Mutate(this, mutationLevel);
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
                Genome speciatedGenome = (Genome)this.Mutate(MutationLevels.INTERSPECIES);
                speciesCollection.Add(i, new List<IGenome>(speciatedGenome.SpawnSiblingGenome(siblingsCount, MutationLevels.CLOSE_SIBLINGS)));
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

        public object Clone()
        {
            Genome clone = (Genome)this.MemberwiseClone();
            clone.Genes = new List<IGenome.IGene>(this.Genes.Select(g => (IGenome.IGene)g.Clone()));
            return clone;
        }

        
    }
}
