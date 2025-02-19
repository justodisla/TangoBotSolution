using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome;
using static TangoBotTrainerApi.IGenome.IGene;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;
using static TangoBotTrainerCoreLib.Genome;

namespace TangoBotTrainerCoreLib.GenomeExtensions
{
    internal static class GenomeStructureHelper
    {
        /// <summary>
        /// Adds a new connection to the genome.
        /// If nodes are supplied, the connection will be between those nodes.
        /// Connection will not allow to connect from a right node to a left node. The connections
        /// allaways go from left to right within the same module.
        /// Connections can go from right to left or remain in the same layer if the nodes are in different modules.
        /// In other words, a node to the right can be origin node and the destination node can be a node to the left if the 
        /// origin node belongs to another module.
        /// A connection should never connect to a node (destination node) in another module.
        /// Connections can have the same node as input and output.
        /// Connections cannot be from a node in the current module to a node in other module.
        /// Connections can be created from a node in other module to a node in the current module.
        /// Connections cannot be created if another connection already exists between the same nodes unless
        /// the previous connection gene is disabled.
        /// from a node in the same module of the genome can connect to nodes in the same module. If the 
        /// toNode is in a different module, the connection should be way less probable.
        /// </summary>
        /// <param name="genome">Current genome</param>
        /// <param name="fromNode">Node from which the connection is connected. If none given, a random node is selected.
        /// The origin node cannot be an output node.
        /// The origin node cannot be disabled
        /// The origin node can belong to other module</param>
        /// <param name="toNode">Node to which the connection is connected.
        /// The method should verify that the destination node is to the left of the origin node
        /// The destination node cannot belong to a different module than the current module
        /// The origin node and destination node cannot participate as nodes of another enabled connection.
        /// The destination node should be to the right of the connected origin node if both belong to the same module</param>
        /// <param name="weight">Determines the connection weight.
        /// if weight is -99, a random weight (double) between -1 and 1 is assigned.</param>
        /// <param name="enabled">Determines if the connection gene is enabled. True by default.
        /// If connection is created disbled, no connection will be done. It will be created without connected nodes.
        /// In other words the ToNode and FromNode properties will be -1</param>
        /// <returns></returns>
        public static IConnectionGene? AddConnection(IGenome genome, INodeGene? fromNode = null, INodeGene? toNode = null, double weight = -99, bool enabled = true)
        {

            if (fromNode == null)
            {
                fromNode = GetRandomNode(genome, new NodeType[] { NodeType.Input, NodeType.Hidden, NodeType.Bias }, restrictToModule: false);
                if (fromNode == null || fromNode.Type == NodeType.Output || !fromNode.Enabled)
                {
                    return null;
                }
            }

            if (toNode == null)
            {
                toNode = GetRandomNode(genome, new NodeType[] { NodeType.Hidden, NodeType.Output }, refLayer: fromNode.Layer, restrictToModule: true, side: "R");
                if (toNode == null || toNode.ModuleId != genome.ModuleId)
                {
                    return null;
                }
            }

            if (ConnectionExists(genome, fromNode, toNode))
            {
                return null;
            }

            if (weight == -99)
            {
                var random = new Random();
                weight = random.NextDouble() * 2 - 1;
            }

            var connection = genome.AddConnection(fromNode.Id, toNode.Id, weight);
            connection.Enabled = enabled;
            return connection;
        }

        /// <summary>
        /// Get a random node from the genome.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="restrictToModule"></param>
        /// <returns></returns>
        public static IGene.IConnectionGene GetRandomConnection(IGenome genome, bool restrictToModule)
        {
            var connections = genome.Genes.OfType<IGene.IConnectionGene>().ToList();
            if (restrictToModule)
            {
                connections = connections.Where(c => c.ModuleId == genome.ModuleId).ToList();
            }

            if (connections.Count == 0)
            {
                return null;
            }

            var random = new Random();
            return connections[random.Next(connections.Count)];
        }
        /// <summary>
        /// Get a random node from the genome.
        /// </summary>
        /// <param name="genome">Genome containing the nodes</param>
        /// <param name="nodeTypes">Types of nodes included</param>
        /// <param name="refLayer">The reference layer to determine which nodes selected fall to which side</param>
        /// <param name="restrictToModule">If nodes selected should be under the same module as the Genome</param>
        /// <param name="side">Indicates which side to search the nodes from. "L" Left, "R" Right and "B" both sides</param>
        /// <returns></returns>
        public static IGene.INodeGene? GetRandomNode(IGenome genome, NodeType[] nodeTypes, int refLayer = -1, bool restrictToModule = true, string side = "B")
        {
            var nodes = genome.Genes.OfType<IGene.INodeGene>().Where(n => nodeTypes.Contains(n.Type)).ToList();
            if (restrictToModule)
            {
                nodes = nodes.Where(n => n.ModuleId == genome.ModuleId).ToList();
            }

            if (refLayer != -1)
            {
                if (side == "L")
                {
                    nodes = nodes.Where(n => n.Layer < refLayer).ToList();
                }
                else if (side == "R")
                {
                    nodes = nodes.Where(n => n.Layer > refLayer).ToList();
                }
            }

            if (nodes.Count == 0)
            {
                return null;
            }

            var random = new Random();
            return nodes[random.Next(nodes.Count)];
        }

        public static bool ConnectionExists(IGenome genome, INodeGene fromNode, INodeGene toNode)
        {
            return genome.Genes.OfType<IGene.IConnectionGene>().Any(c => c.FromNode == fromNode.Id && c.ToNode == toNode.Id);
        }

        /// <summary>
        /// Adds a new node to the genome.
        /// If the new node is disabled, no connections will be created.
        /// The layer where the node is added is random from 1 to 99. Since layer 0 is reserved for input nodes and layer 100 is reserved for output nodes.
        /// New nodes should be connected from a node to the left that can be an input node or a hidden node.
        /// New nodes can have connections from another node in another module but cannot be connected to a node in another module except output node.
        /// New nodes can fall amid an existing connection. In this case, the existing connection changes the ToNode for the new node
        /// and a new connection is created to connect the new node to the node formerly connected to the severed connection. The wight of the 
        /// new connection is the same as the severed connection.
        /// Special nodes (input, output, bias) cannot be added.
        /// New nodes can be connected from and to nodes in the same layer.
        /// New nodes can have one recoursive connecton to itself.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="nodeType"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public static INodeGene? AddNode(IGenome genome, NodeType nodeType = NodeType.Hidden, bool enabled = true)
        {
            if (nodeType == NodeType.Input || nodeType == NodeType.Output || nodeType == NodeType.Bias || nodeType == NodeType.Special)
            {
                return null;
            }

            var random = new Random();
            int layer = random.Next(1, 100);

            var newNode = genome.AddNewNode();// new NodeGene(nodeType, layer); // Create a new node gene instance
            newNode.Enabled = enabled;
            genome.Genes.Add(newNode); // Add the new node to the genome's genes

            if (enabled)
            {
                var fromNode = GetRandomNode(genome, new NodeType[] { NodeType.Input, NodeType.Hidden }, refLayer: layer, restrictToModule: false, side: "L");
                if (fromNode != null)
                {
                    genome.AddConnection(fromNode.Id, newNode.Id, random.NextDouble() * 2 - 1);
                }

                var toNode = GetRandomNode(genome, new NodeType[] { NodeType.Hidden, NodeType.Output }, refLayer: layer, restrictToModule: true, side: "R");
                if (toNode != null)
                {
                    genome.AddConnection(newNode.Id, toNode.Id, random.NextDouble() * 2 - 1);
                }
            }

            return newNode;
        }
    }
}
