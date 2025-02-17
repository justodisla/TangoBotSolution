using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    internal partial class Genome
    {
        /// <summary>
        /// Compares two genomes for equality in terms of the number of genes, types of genes, and inner gene values.
        /// </summary>
        /// <param name="otherGenome">The other genome to compare with.</param>
        /// <returns>True if the genomes are equal, otherwise false.</returns>
        public bool CompareGenomes(IGenome otherGenome)
        {
            if (otherGenome == null)
            {
                throw new ArgumentNullException(nameof(otherGenome));
            }

            // Compare the number of genes
            if (this.Genes.Count != otherGenome.Genes.Count)
            {
                return false;
            }

            // Compare the values of the inner genes
            for (int i = 0; i < this.Genes.Count; i++)
            {
                var thisGene = this.Genes[i];
                var otherGene = otherGenome.Genes[i];

                if (!CompareGenes(thisGene, otherGene))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two genes for equality, including their types.
        /// </summary>
        /// <param name="gene1">The first gene to compare.</param>
        /// <param name="gene2">The second gene to compare.</param>
        /// <returns>True if the genes are equal, otherwise false.</returns>
        private bool CompareGenes(IGenome.IGene gene1, IGenome.IGene gene2)
        {
            if (gene1 == null || gene2 == null)
            {
                return false;
            }

            // Compare types
            if (gene1.GetType() != gene2.GetType())
            {
                return false;
            }

            // Compare basic properties
            if (gene1.Id != gene2.Id ||
                gene1.InnovationNumber != gene2.InnovationNumber ||
                gene1.ModuleId != gene2.ModuleId ||
                gene1.Enabled != gene2.Enabled)
            {
                return false;
            }

            // Compare node genes
            if (gene1 is IGenome.IGene.INodeGene nodeGene1 && gene2 is IGenome.IGene.INodeGene nodeGene2)
            {
                if (nodeGene1.Type != nodeGene2.Type ||
                    nodeGene1.Layer != nodeGene2.Layer ||
                    nodeGene1.Bias != nodeGene2.Bias)
                {
                    return false;
                }
            }

            // Compare connection genes
            if (gene1 is IGenome.IGene.IConnectionGene connectionGene1 && gene2 is IGenome.IGene.IConnectionGene connectionGene2)
            {
                if (connectionGene1.FromNode != connectionGene2.FromNode ||
                    connectionGene1.ToNode != connectionGene2.ToNode ||
                    connectionGene1.Weight != connectionGene2.Weight)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
