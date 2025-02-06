using TangoBotTrainerApi;
using TangoBotTrainerLib;
using TangoBotTrainerLib.Data;

class Program
{
    static void Main(string[] args)
    {

        // Create runtime
        var runtime = new Runtime();

        // Set training data
        var trainingData = new TrainingDataComponent(); // Placeholder
        runtime.SetTrainingData(trainingData);

        // Create agent and supervisor
        var agent = new Agent("TradingPlatform", new IPerceptor[0], new IActuator[0]);
        var supervisor = new Supervisor();

        // Start campaign
        var campaign = runtime.StartCampaign(agent, supervisor);

        // Get result
        var neuralNetwork = campaign.GetResult();
        Console.WriteLine("Neural network trained successfully.");
    }
}