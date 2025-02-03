using System;
using System.Collections.Generic;

public class Genome
{
    public List<Node> Nodes { get; private set; }
    public List<Connection> Connections { get; private set; }
    public double Fitness { get; set; }

    public Genome(List<Node> nodes, List<Connection> connections)
    {
        Nodes = nodes;
        Connections = connections;
        Fitness = 0;
    }

    public double[] Evaluate(double[] inputs)
    {
        if (inputs.Length != Nodes.Count(n => n.Type == NodeType.Input))
            throw new ArgumentException("Input length does not match the number of input nodes.");

        // Reset activations
        foreach (var node in Nodes)
        {
            node.Activation = 0;
        }

        // Set input activations
        int inputIndex = 0;
        foreach (var node in Nodes)
        {
            if (node.Type == NodeType.Input)
            {
                node.Activation = inputs[inputIndex++];
            }
        }

        // Forward pass through the network
        foreach (var connection in Connections)
        {
            if (connection.IsEnabled)
            {
                var sourceNode = Nodes.Find(n => n.Id == connection.SourceNodeId);
                var targetNode = Nodes.Find(n => n.Id == connection.TargetNodeId);

                if (sourceNode != null && targetNode != null)
                {
                    targetNode.Activation += sourceNode.Activation * connection.Weight;
                }
            }
        }

        // Apply activation function (e.g., sigmoid) to output nodes
        var outputs = new List<double>();
        foreach (var node in Nodes)
        {
            if (node.Type == NodeType.Output)
            {
                node.Activation = Sigmoid(node.Activation); // Apply sigmoid activation
                outputs.Add(node.Activation);
            }
        }

        return outputs.ToArray();
    }

    private double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }
}