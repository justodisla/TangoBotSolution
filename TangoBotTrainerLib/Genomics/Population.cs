using System;
using System.Collections.Generic;

public class Population
{
    public List<Genome> Genomes { get;  set; }

    public Population(int populationSize, int inputCount, int outputCount)
    {
        Genomes = new List<Genome>();

        Random random = new Random();

        for (int i = 0; i < populationSize; i++)
        {
            // Create input and output nodes
            var nodes = new List<Node>();
            for (int j = 0; j < inputCount; j++)
                nodes.Add(new Node(j, NodeType.Input));

            for (int j = 0; j < outputCount; j++)
                nodes.Add(new Node(inputCount + j, NodeType.Output));

            // Create fully connected initial topology
            var connections = new List<Connection>();
            foreach (var inputNode in nodes.FindAll(n => n.Type == NodeType.Input))
            {
                foreach (var outputNode in nodes.FindAll(n => n.Type == NodeType.Output))
                {
                    connections.Add(new Connection(
                        inputNode.Id,
                        outputNode.Id,
                        random.NextDouble() * 2 - 1 // Random weight in [-1, 1]
                    ));
                }
            }

            Genomes.Add(new Genome(nodes, connections));
        }
    }
}