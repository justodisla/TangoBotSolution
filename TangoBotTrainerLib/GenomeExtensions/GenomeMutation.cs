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

                List<IGene> genesToMutate = [];
                bool ignoreDifferentModule = false;

                foreach (var gene in shuffledGenes)
                {
                    ignoreDifferentModule = RandomizeHelper.GenerateRandomBool(0.1);
                    int geneModule = gene.ModuleId;
                    if (ModuleId != geneModule)
                    {
                        if (!ignoreDifferentModule)
                        {
                            continue;
                        }
                    }

                    if (RandomizeHelper.GenerateRandomBool(valueMutationChange))
                        genesToMutate.Add(gene);
                }

                if (genesToMutate.Count == 0 && shuffledGenes.Count > 0)
                {
                    ignoreDifferentModule = RandomizeHelper.GenerateRandomBool(0.1);
                    foreach (var gene in shuffledGenes)
                    {
                        if (gene.ModuleId == this.ModuleId && ignoreDifferentModule)
                        {
                            genesToMutate.Add(gene);
                            break;
                        }
                    }
                }

                //Let's do structural mutations
                //Let's remove genes.
                List<IGene> genesToRemove = [];
                foreach (var geneToDisable in shuffledGenes)
                {
                    ignoreDifferentModule = RandomizeHelper.GenerateRandomBool(structuralMutationChange);
                    bool removeGene = RandomizeHelper.GenerateRandomBool(structuralMutationChange);
                    bool isDifferentModule = geneToDisable.ModuleId != this.ModuleId;

                    if (removeGene && (!isDifferentModule || ignoreDifferentModule))
                    {
                        genesToRemove.Add(geneToDisable);
                    }
                }

                foreach (var item in genesToRemove)
                {
                    item.Enabled = false;
                }

                //Let's add new genes
                bool addGenes = RandomizeHelper.GenerateRandomBool(structuralMutationChange);

                if(addGenes)
                {
                    int genesToAdd = RandomizeHelper.GenerateRandomInt(1, 3);
                    for (global::System.Int32 i = 0; i < genesToAdd; i++)
                    {
                        copiedGenome.AddRandomGene(mutationLevel);
                    }
                }

                copiedGenome.FixStructure();

                //************************

                if (genesToMutate.Count == 0 && shuffledGenes.Count > 0)
                {
                    genesToMutate.Add(shuffledGenes[RandomizeHelper.GenerateRandomInt(0, (shuffledGenes.Count - 1))]);
                }

                foreach (var item in genesToMutate)
                {
                    item.Mutate(mutationLevel);
                }

                double disturbance = (RandomizeHelper.GenerateRandomDouble(0, valueMutationChange));

                double v = shuffledGenes.Count * disturbance;
                double v1 = Math.Round(v, MidpointRounding.ToPositiveInfinity);
                int numberOfGenesToMutate = (int)v1;// (int)(RandomizeHelper.GenerateRandomDouble(0, 1) * shuffledGenes.Count * valueMutationChange);

                for (int i = 0; i < numberOfGenesToMutate; i++)
                {
                    IGene gene = shuffledGenes[i];
                    StructuralChange change = GeneticOperator.SelectStructuralChange();

                    /*
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
                    */
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

                /*
                // Shuffle the Genes collection
                List<IGene> shuffledGenes = copiedGenome.Genes
                    .Where(g => !(g is IGene.INodeGene nodeGene && (nodeGene.Type == NodeType.Input || nodeGene.Type == NodeType.Output)))
                    .OrderBy(x => RandomizeHelper.GenerateRandomDouble(0, 1))
                    .ToList();
                */


                //copiedGenome.MutateStructure(mutationLevel);


            }



            /*
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
            */

            copiedGenome.FixStructure();

            return copiedGenome;
        }
    }
}
