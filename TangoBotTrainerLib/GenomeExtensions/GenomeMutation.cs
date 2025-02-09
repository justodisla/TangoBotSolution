using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;

namespace TangoBotTrainerCoreLib
{
    internal partial class Genome : IGenome
    {
        public int CurrentInnovationNumber { get; set; }

        internal int GetNextInnovationNumber()
        {
            CurrentInnovationNumber++;
            return CurrentInnovationNumber;
        }

        public IGenome Mutate(MutationLevels mutationLevel)
        {
            double valueMutationChange;
            double structuralMutationChange = 0;

            switch (mutationLevel)
            {
                case MutationLevels.DEFAULT:
                    valueMutationChange = 0.1;
                    structuralMutationChange = 0.001;
                    break;
                case MutationLevels.CLOSE_SIBLINGS:
                    valueMutationChange = 0.05;
                    structuralMutationChange = 0.01;
                    break;
                case MutationLevels.DISTANT_SIBLINGS:
                    valueMutationChange = 0.2;
                    structuralMutationChange = 0.09;
                    break;
                case MutationLevels.EXTREME:
                    valueMutationChange = 0.3;
                    structuralMutationChange = 0.25;
                    break;
                case MutationLevels.INTERSPECIES:
                    valueMutationChange = 0.8;
                    structuralMutationChange = 0.5;
                    break;
                case MutationLevels.RANDOM:
                    valueMutationChange = RandomizeHelper.GenerateRandomDouble(0, 1);
                    structuralMutationChange = 0.001;
                    break;
            }

            // Shuffle the Genes collection
            List<IGene> shuffledGenes = Genes
                .Where(g => !(g is IGene.INodeGene nodeGene && (nodeGene.Type == NodeType.Input || nodeGene.Type == NodeType.Output)))
                .OrderBy(x => RandomizeHelper.GenerateRandomDouble(0, 1))
                .ToList();

            // Mutate the existing genes
            if (shuffledGenes.Count > 0)
            {
                int randomIndex = (int)(RandomizeHelper.GenerateRandomDouble(0, 1) * shuffledGenes.Count * structuralMutationChange);

                for (int i = 0; i < randomIndex; i++)
                {
                    IGene gene = shuffledGenes[i];
                    // gene.Mutate(mutationLevel);
                }
            }

            // If there are no connections, the genome is basic and new connections have to be added
            bool isBasicGenome = Genes.All(g => g is IGene.INodeGene nodeGene && (nodeGene.Type == NodeType.Input || nodeGene.Type == NodeType.Output));

            // Mutate the structure of the genome
            if (isBasicGenome)
            {

                int possibleConnections = Genes.OfType<IGene.INodeGene>().Count(n => n.Type == NodeType.Input) *
                                         Genes.OfType<IGene.INodeGene>().Count(n => n.Type == NodeType.Output);

                double randomPercentage = RandomizeHelper.GenerateRandomDouble(0, 1);
                int connectionsToAdd = Math.Max(1, (int)(possibleConnections * randomPercentage));

                for (int i = 0; i < connectionsToAdd; i++)
                {
                    AddNewConnection();
                }

                //Let's add some nodes
                bool canAddNewNode = RandomizeHelper.GenerateRandomBool(0.5);

                int nodesToAdd = canAddNewNode ? RandomizeHelper.GenerateRandomInt(1, 3) : 0;

                for (int i = 0; i < nodesToAdd; i++)
                {
                    AddNewNode();
                }

                Console.WriteLine($"Adding {connectionsToAdd} connections to the genome.");
            }

            FixStructure();

            return (IGenome)this.Clone();
        }
    }
}
