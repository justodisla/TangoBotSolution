using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;
using static TangoBotTrainerApi.IGenome;

namespace TangoBotTrainerCoreLib
{
    internal partial class Genome : IGenome
    {
        public IGene.INodeGene AddNode(NodeType type, int layer, double bias = 0)
        {
            int targetLayer = layer != -1 ? layer : RandomizeHelper.GenerateRandomInt(1, 100);
            int module = 1;
            double b = bias != 0 ? bias : RandomizeHelper.GenerateRandomDouble(-1, 1);
            int innovationNumber = GetNextInnovationNumber();
            int id = RandomizeHelper.GenerateUniqueId();

            bool cutConnection = RandomizeHelper.GenerateRandomBool(0.1);

            if (cutConnection)
            {
                //Cut a connection and add a node
                IGenome.IGene.IConnectionGene c = GetRandomConnection();
            }

            IGenome.IGene.INodeGene n = new NodeGene(id, innovationNumber, module, true, NodeType.Hidden, targetLayer, this);

            return n;
        }

        /// <summary>
        /// Get a random connection from the genome.
        /// </summary>
        /// <returns></returns>
        public IGene.IConnectionGene GetRandomConnection()
        {
            return Genes[RandomizeHelper.GenerateRandomInt(0, Genes.Count - 1)] as IGene.IConnectionGene;
        }

        public void AddConnection()
        {
            int fromNode = RandomizeHelper.GenerateRandomInt(0, Genes.Count - 1);
            int toNode = RandomizeHelper.GenerateRandomInt(0, Genes.Count - 1);
            double weight = RandomizeHelper.GenerateRandomDouble(-1, 1);
            AddConnection(fromNode, toNode, weight);
        }
        public IGene.IConnectionGene AddConnection(int fromNode, int toNode, double weight)
        {
            int innovationNumber = GetNextInnovationNumber();
            int id = RandomizeHelper.GenerateUniqueId();
            int module = 1;
            IGenome.IGene.IConnectionGene c = new ConnectionGene(id, innovationNumber, module, true, fromNode, toNode, weight, this);
            return c;
        }

        /// <summary>
        /// Get a random node of any kind from the genome.
        /// </summary>
        /// <returns></returns>
        public IGene.INodeGene GetRandomNode(bool includeHidden = true, bool includeInput = true, bool includeOutput = true)
        {
            return Genes[RandomizeHelper.GenerateRandomInt(0, Genes.Count - 1)] as IGene.INodeGene;
        }

        
        public void RemoveConnection(IGene.IConnectionGene connection)
        {
            connection.Enabled = false;
        }

        public void RemoveNode(IGene.INodeGene node)
        {
            node.Enabled = false;
        }

        public void GetRandomNodeToTheRight(int fromLayer = 0)
        {
            gene.Enabled = false;
        }


    }
}
