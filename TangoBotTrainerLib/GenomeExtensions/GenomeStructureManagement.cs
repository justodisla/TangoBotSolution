using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;
using static TangoBotTrainerApi.IGenome;
using System.Data;

namespace TangoBotTrainerCoreLib
{
    internal partial class Genome : IGenome
    {

        public IGene.INodeGene AddNode(NodeType type, int layer, double bias = 0)
        {
            int targetLayer = layer != -1 ? layer : RandomizeHelper.GenerateRandomInt(1, 100);
            int module = ModuleId;
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

            Genes.Add(n);

            //FixStructure();

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

        public void AddNewConnection()
        {
            double weight = RandomizeHelper.GenerateRandomDouble(-1, 1);

            this.Genes.Add(
                new ConnectionGene(
                    RandomizeHelper.GenerateUniqueId(), 
                    GetNextInnovationNumber(), 
                    ModuleId, 0, 0, weight, true, this));

            /*
            bool connectionExists = true;
            while (connectionExists)
            {
                var fromNode = GetRandomNode(true, true, false);// RandomizeHelper.GenerateRandomInt(0, Genes.Count - 1);
                var toNode = GetRandomNodeToTheRight(fromNode.Layer, false, true, true);// RandomizeHelper.GenerateRandomInt(0, Genes.Count - 1);
                connectionExists = ConnectionExists(fromNode, toNode);

                if (!connectionExists)
                    AddConnection(fromNode.Id, toNode.Id, weight);

            }
            */
        }
        public void AddNewNode()
        {
            int targetLayer = RandomizeHelper.GenerateRandomInt(1, 100);
            //int module = ModuleId;
            double bias = RandomizeHelper.GenerateRandomDouble(-1, 1);
            NodeType nType = NodeType.Hidden;

            AddNode(nType, targetLayer, bias);

        }

        private bool ConnectionExists(IGene.INodeGene fromNode, IGene.INodeGene toNode)
        {
            return Genes.Any(g => g is IGene.IConnectionGene connectionGene &&
                                              connectionGene.FromNode == fromNode.Id &&
                                              connectionGene.ToNode == toNode.Id);
        }

        [Obsolete("Use AddNewConnection instead")]
        public IGene.IConnectionGene AddConnection(int fromNode, int toNode, double weight)
        {
            int innovationNumber = GetNextInnovationNumber();
            int id = RandomizeHelper.GenerateUniqueId();
            int module = ModuleId;
            IGenome.IGene.IConnectionGene c = new ConnectionGene(id, innovationNumber, module, fromNode, toNode, weight, true, this);

            Genes.Add(c);

            return c;
        }

        /// <summary>
        /// Get a random node of any kind from the genome.
        /// </summary>
        /// <returns></returns>
        public IGene.INodeGene GetRandomNode(bool includeHidden = true, bool includeInput = true, bool includeOutput = true)
        {
            List<IGene.INodeGene> nodes = GetAllNodes(includeInput, includeOutput, includeHidden);
            return nodes[RandomizeHelper.GenerateRandomInt(0, nodes.Count - 1)];
        }

        public List<IGene.INodeGene> GetAllNodes(bool includeInput = true, bool includeOutput = true, bool includeHidden = true)
        {
            return this.Genes
                .Where(g => g is IGene.INodeGene nodeGene &&
                            ((includeInput && nodeGene.Type == NodeType.Input) ||
                             (includeOutput && nodeGene.Type == NodeType.Output) ||
                             (includeHidden && nodeGene.Type == NodeType.Hidden)))
                .Cast<IGene.INodeGene>()
                .ToList();
        }

        public void RemoveConnection(IGene.IConnectionGene connection)
        {
            connection.Enabled = false;
        }

        public void RemoveNode(IGene.INodeGene node)
        {
            node.Enabled = false;
        }

        /// <summary>
        /// Returns a random node to the right of the current layer
        /// </summary>
        /// <param name="referenceLayer"></param>
        public IGene.INodeGene GetRandomNodeInDirection(
            int referenceLayer = 0,
            bool includeInput = true,
            bool includeHidden = true,
            bool includeOutput = true,
            int layersToLook = -1,
            bool toTheRight = true)
        {

            var nodes = this.GetAllNodes(includeInput, includeOutput, includeHidden)
                .Where(node => toTheRight
                ? node.Layer > referenceLayer && node.Layer > referenceLayer && node.Type != NodeType.Input
                : node.Layer < referenceLayer && node.Layer < referenceLayer && node.Type != NodeType.Output)
                .ToList();

            if (layersToLook != -1)
            {
                nodes = nodes.Where(node => Math.Abs(node.Layer - referenceLayer) <= layersToLook).ToList();
            }

            if (nodes.Count == 0)
                return null;

            int randomIndex = RandomizeHelper.GenerateRandomInt(0, nodes.Count - 1);
            return nodes[randomIndex];
        }

        public IGene.INodeGene GetRandomNodeToTheRight(int fromLayer = 0, bool includeInput = true, bool includeHidden = true, bool includeOutput = true, int layersToLook = -1)
        {
            return GetRandomNodeInDirection(fromLayer, includeInput, includeHidden, includeOutput, layersToLook, true);
        }

        public IGene.INodeGene GetRandomNodeToTheLeft(int fromLayer = 0, bool includeInput = true, bool includeHidden = true, bool includeOutput = true, int layersToLook = -1)
        {
            return GetRandomNodeInDirection(fromLayer, includeInput, includeHidden, includeOutput, layersToLook, false);
        }

        /// <summary>
        /// This method searches for nodes and connections that because of structural
        /// modifications, ended up partially or totally disconected.
        /// Nodes might end up orphan or leaf. Connection might end up without from or to connection
        /// This is what this method should fix
        /// </summary>
        public void FixStructure()
        {
            //Fix orphan nodes
            //Fix leaf nodes
            //Fix broken connections

            List<IGene> danglingConnections = Genes.Where(g => {
                return g is IGene.IConnectionGene cGene && 
                (cGene.FromNode == 0 || cGene.ToNode == 0);
            }).ToList();

            foreach (IGene.IConnectionGene item in danglingConnections)
            {
                int originNodeId = item.FromNode;
                int destinationNodeId = item.ToNode;

                bool doOriginNodeConnection = originNodeId == 0;
                bool doDestinationNodeConnection = destinationNodeId == 0;
                bool doBothConnections = doOriginNodeConnection && doDestinationNodeConnection;

                if (doBothConnections)
                {
                    IGene.INodeGene originNode = GetRandomNode(true, true, false);
                    int layer = originNode.Layer;
                    IGene.INodeGene destinationNode = GetRandomNodeToTheRight(layer, false, true, true);

                    item.FromNode = originNode.Id;
                    item.ToNode = destinationNode.Id;
                }
                else
                {
                    if (doDestinationNodeConnection)
                    {
                        IGene.INodeGene? originNode = GetAllNodes().Find(n => n.Id == item.FromNode);
                        if (originNode == null)
                        {
                            originNode = GetRandomNode(true, true, false);
                            item.FromNode = originNode.Id;
                        }
                        IGene.INodeGene destinationNode = GetRandomNodeToTheRight(originNode.Layer, false, true, true);
                        item.ToNode = destinationNode.Id;
                    }

                    if (doOriginNodeConnection)
                    {
                        IGene.INodeGene? destinationNode = GetAllNodes().Find(n => n.Id == item.ToNode);
                        if (destinationNode == null)
                        {
                            destinationNode = GetRandomNode(true, true, false);
                            item.ToNode = destinationNode.Id;
                        }
                        IGene.INodeGene originNode = GetRandomNodeToTheLeft(destinationNode.Layer, true, true, false);
                        item.FromNode = originNode.Id;


                    }
                    



                }

            bool changesMade;
            do
            {
                changesMade = false;

                // Find and fix orphan nodes
                var orphanNodes = GetAllNodes(includeInput: false, includeOutput: false, includeHidden: true)
                    .Where(node => !node.GetIncomingConnections().Any())
                    .ToList();

                foreach (var orphanNode in orphanNodes)
                {
                    var nodeToTheLeft = GetRandomNodeToTheLeft(orphanNode.Layer);
                    if (nodeToTheLeft != null)
                    {
                        AddConnection(nodeToTheLeft.Id, orphanNode.Id, RandomizeHelper.GenerateRandomDouble(-1, 1));
                        changesMade = true;
                    }
                }

                // Find and fix leaf nodes
                var leafNodes = GetAllNodes(includeInput: false, includeOutput: false, includeHidden: true)
                    .Where(node => !node.GetOutGoingConnections().Any())
                    .ToList();

                foreach (var leafNode in leafNodes)
                {
                    var nodeToTheRight = GetRandomNodeToTheRight(leafNode.Layer);
                    if (nodeToTheRight != null)
                    {
                        AddConnection(leafNode.Id, nodeToTheRight.Id, RandomizeHelper.GenerateRandomDouble(-1, 1));
                        changesMade = true;
                    }
                }

                // Find and fix broken connections
                var brokenConnections = Genes.OfType<IGene.IConnectionGene>()
                    .Where(conn => !Genes.OfType<IGene.INodeGene>().Any(node => node.Id == conn.FromNode) ||
                                   !Genes.OfType<IGene.INodeGene>().Any(node => node.Id == conn.ToNode))
                    .ToList();

                foreach (var connection in brokenConnections)
                {
                    if (RandomizeHelper.GenerateRandomBool(0.5))
                    {
                        // Randomly disable the connection
                        connection.Enabled = false;
                        changesMade = true;
                        continue;
                    }

                    if (!Genes.OfType<IGene.INodeGene>().Any(node => node.Id == connection.FromNode))
                    {
                        // Fix connection without FROM node
                        var nodeToTheLeft = GetRandomNodeToTheRight(connection.ToNode - 1);
                        if (nodeToTheLeft != null)
                        {
                            connection.FromNode = nodeToTheLeft.Id;
                            changesMade = true;
                        }
                    }

                    if (!Genes.OfType<IGene.INodeGene>().Any(node => node.Id == connection.ToNode))
                    {
                        // Fix connection without TO node
                        var nodeToTheRight = GetRandomNodeToTheRight(connection.FromNode);
                        if (nodeToTheRight != null)
                        {
                            connection.ToNode = nodeToTheRight.Id;
                            changesMade = true;
                        }
                    }
                }

            } while (changesMade);
        }

        public void AddRandomGene(MutationLevels mutationLevel)
        {
            //Decide which type of node
            string[] nodeTypes = ["N", "C"];
            switch (nodeTypes[RandomizeHelper.GenerateRandomInt(0, nodeTypes.Length - 1)])
            {
                case "N":
                    AddNewNode();
                    break;
                case "C":
                    AddNewConnection();
                    break;
                default:
                    break;
            }

        }
    }
}
