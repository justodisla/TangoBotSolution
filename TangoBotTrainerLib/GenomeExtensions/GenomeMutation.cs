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
            //The genome that is going to be the mutated clone of the current genome
            IGenome copiedGenome = (IGenome)this.Clone();

            // If there are no connections, the genome is basic and new connections have to be added
            bool isBasicGenome = copiedGenome.Genes.All(g => g is IGene.INodeGene nodeGene && (nodeGene.Type == NodeType.Input || nodeGene.Type == NodeType.Output));

            // Is a basic genome which means that basic structure has to be added
            if (isBasicGenome)
            {
                //Let's add some connections
                int possibleConnections = copiedGenome.Genes.OfType<IGene.INodeGene>().Count(n => n.Type == NodeType.Input) *
                                         copiedGenome.Genes.OfType<IGene.INodeGene>().Count(n => n.Type == NodeType.Output);

                double randomPercentage = RandomizeHelper.GenerateRandomDouble(0, 1);

                //Determines how many connections to add
                int connectionsToAdd = Math.Max(1, (int)(possibleConnections * randomPercentage));

                //Add the connections
                for (int i = 0; i < connectionsToAdd; i++)
                {
                    copiedGenome.AddNewConnection();
                }

                //Let's add some nodes
                //Determines if a new nodes are to be added
                bool canAddNewNode = RandomizeHelper.GenerateRandomBool(0.5);

                //Determines how many nodes to add
                int nodesToAdd = canAddNewNode ? RandomizeHelper.GenerateRandomInt(1, 3) : 0;

                //Add the nodes
                for (int i = 0; i < nodesToAdd; i++)
                {
                    copiedGenome.AddNewNode();
                }

                return copiedGenome;

                Console.WriteLine($"Adding {connectionsToAdd} connections to the genome.");
            }
            else //Is not a basic genome
            {
                //Prepare to do mutations
                double valueMutationChange = GeneticOperator.ResolveMutationLevelValue(mutationLevel).Item1;
                double structuralMutationChange = GeneticOperator.ResolveMutationLevelValue(mutationLevel).Item2;

                List<IGene> shuffledGenes = copiedGenome.Genes
                    .Where(g => !(g is IGene.INodeGene nodeGene && (nodeGene.Type == NodeType.Input || nodeGene.Type == NodeType.Output)))
                    .OrderBy(x => RandomizeHelper.GenerateRandomDouble(0, 1))
                    .ToList();

                int numberOfGenesToMutate = (int)(RandomizeHelper.GenerateRandomDouble(0, 1) * shuffledGenes.Count * valueMutationChange);

                for (int i = 0; i < numberOfGenesToMutate; i++)
                {
                    IGene gene = shuffledGenes[i];
                    StructuralChange change = GeneticOperator.SelectStructuralChange();

                    switch(change)
                    {
                        case StructuralChange.ADD://Adds a gene
                            copiedGenome.AddNewConnection();
                            break;
                        case StructuralChange.REMOVE://Removes the current gene
                            copiedGenome.AddGene();
                            break;
                        case StructuralChange.RECONNECT://Reconnects the gene

                            copiedGenome.RemoveConnection();
                            break;
                        case StructuralChange.REMOVE_NODE:
                            copiedGenome.RemoveNode();
                            case StructuralChange.DO_NOTHING:

                            break;
                        default:
                            break;
                    }

                    gene.Mutate(mutationLevel);
                }


                //Attempt to mutate the structure of the genome
                List<IGene.IConnectionGene> selectedGenes = copiedGenome.Genes
                    .Where(g => g is IGene.IConnectionGene)
                    .OrderBy(x => RandomizeHelper.GenerateRandomDouble(0, 1))
                    .Cast<IGene.IConnectionGene>()
                    .ToList();

                int numberOfConnectionGenesToMutate = (int)(RandomizeHelper.GenerateRandomDouble(0, 1) * selectedGenes.Count * structuralMutationChange);

                for (int i = 0; i < numberOfConnectionGenesToMutate; i++)
                {
                    IGene gene = selectedGenes[i];
                    gene.Mutate(mutationLevel);
                }


                // Shuffle the Genes collection
                List<IGene> shuffledGenes = copiedGenome.Genes
                    .Where(g => !(g is IGene.INodeGene nodeGene && (nodeGene.Type == NodeType.Input || nodeGene.Type == NodeType.Output)))
                    .OrderBy(x => RandomizeHelper.GenerateRandomDouble(0, 1))
                    .ToList();

                

                //copiedGenome.MutateStructure(mutationLevel);


            }


            

            // Mutate the existing genes
            if (shuffledGenes.Count > 0)
            {
                int numberOfGenesToMutate = (int)(RandomizeHelper.GenerateRandomDouble(0, 1) * shuffledGenes.Count * valueMutationChange);

                for (int i = 0; i < numberOfGenesToMutate; i++)
                {
                    IGene gene = shuffledGenes[i];
                    gene.Mutate(mutationLevel);
                }
            }

            

            

            copiedGenome.FixStructure();

            return copiedGenome;
        }
    }
}
