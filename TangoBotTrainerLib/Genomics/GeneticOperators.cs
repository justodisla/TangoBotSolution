public static class GeneticOperators
{
    private static Random random = new Random();

    public static void Mutate(Genome genome, double mutationRate = 0.1, double mutationStrength = 0.1)
    {
        // Mutate connection weights
        foreach (var connection in genome.Connections)
        {
            if (random.NextDouble() < mutationRate)
            {
                connection.Weight += (random.NextDouble() * 2 - 1) * mutationStrength;
            }
        }

        // Add a new node (structural mutation)
        if (random.NextDouble() < mutationRate)
        {
            AddNode(genome);
        }

        // Add a new connection (structural mutation)
        if (random.NextDouble() < mutationRate)
        {
            AddConnection(genome);
        }
    }

    private static void AddNode(Genome genome)
    {
        // Placeholder for adding a new node
    }

    private static void AddConnection(Genome genome)
    {
        // Placeholder for adding a new connection
    }

    public static Genome Crossover(Genome parent1, Genome parent2)
    {
        // Placeholder for crossover logic
        return new Genome(new List<Node>(), new List<Connection>());
    }
}