using System;
using System.Collections.Generic;

public class EvolutionManager
{
    private static Random random = new Random();

    public void Evolve(Population population, List<MarketState> marketStates, int generations)
    {
        var evaluator = new FitnessEvaluator();

        for (int generation = 0; generation < generations; generation++)
        {
            // Evaluate fitness for each genome
            foreach (var genome in population.Genomes)
            {
                genome.Fitness = evaluator.Evaluate(genome, marketStates);
            }

            // Sort genomes by fitness (descending order: highest fitness first)
            population.Genomes.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

            Console.WriteLine($"Generation {generation + 1}: Best Fitness = {population.Genomes[0].Fitness}");

            // Create next generation
            var nextGeneration = new List<Genome>();

            // Elitism: Keep the top-performing genomes (e.g., top 10%)
            int eliteCount = population.Genomes.Count / 10;
            for (int i = 0; i < eliteCount; i++)
            {
                nextGeneration.Add(population.Genomes[i]);
            }

            // Generate the rest of the population through crossover and mutation
            for (int i = eliteCount; i < population.Genomes.Count; i++)
            {
                var parent1 = SelectParent(population.Genomes);
                var parent2 = SelectParent(population.Genomes);

                var child = GeneticOperators.Crossover(parent1, parent2);
                GeneticOperators.Mutate(child);

                nextGeneration.Add(child);
            }

            population.Genomes = nextGeneration;
        }
    }

    private Genome SelectParent(List<Genome> genomes)
    {
        // Tournament selection: Randomly select two genomes and pick the fitter one
        var candidate1 = genomes[random.Next(genomes.Count)];
        var candidate2 = genomes[random.Next(genomes.Count)];
        return candidate1.Fitness > candidate2.Fitness ? candidate1 : candidate2;
    }
}