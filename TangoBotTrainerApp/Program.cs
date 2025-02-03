using TangoBotTrainerLib.Data;

class Program
{
    static async Task Main(string[] args)
    {
        Agent agent = new Agent();

       // agent.Inputs.Add("LastPrice");
        //agent.Outputs.add("Buy");

        // Initialize population
        var population = new Population(populationSize: 100, inputCount: 8, outputCount: 7);

        // Fetch and process market data
        var marketData = new MarketData();
        var rawData = await marketData.FetchHistoricalData("AAPL", "A5069SA46CZPGTHQ");
        var processor = new DataProcessor();
        var marketStates = processor.ProcessData(rawData, period: 14);

        // Evolve population
        var evolutionManager = new EvolutionManager();
        evolutionManager.Evolve(population, marketStates, generations: 50);

        // Retrieve best genome
        var bestGenome = population.Genomes[0];
        Console.WriteLine($"Best Genome Fitness: {bestGenome.Fitness}");
    }
}