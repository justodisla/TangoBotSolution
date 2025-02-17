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

        public int ModuleId { get; set; }

        public Genome(IAgent agent)
        {
            Genes ??= [];

            //Create the input nodes
            if (agent == null)
            {
                throw new ArgumentNullException();
            }

            Agent = agent;

            foreach (IPerceptor p in agent.GetPercetors())
            {
                IGenome.IGene.INodeGene n = new NodeGene(RandomizeHelper.GenerateUniqueId(), -1, -1, true, NodeType.Input, 0, this);
                Genes.Add(n);
            }

            foreach (IActuator p in agent.GetActuators())
            {
                IGenome.IGene.INodeGene n = new NodeGene(RandomizeHelper.GenerateUniqueId(), -1, -1, true, NodeType.Output, 99, this);
                Genes.Add(n);
            }
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
                IGenome[] siblings = speciatedGenome.SpawnSiblingGenomes(siblingsCount, MutationLevels.CLOSE_SIBLINGS);
                speciesCollection.Add(i, new List<IGenome>(siblings));

                foreach (IGenome g in speciesCollection[i])
                {
                   var hc =  g.GetHashCode();
                    Console.WriteLine(hc);
                }

            }

            return speciesCollection.SelectMany(x => x.Value).ToArray();
        }

        public IGenome[] SpawnSiblingGenomes(int count, MutationLevels mutationLevel)
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
                IGenome mutatedGenome = this.Mutate(mutationLevel);

                bool cmp= CompareGenomes(mutatedGenome);

                siblings.Add(mutatedGenome);

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
