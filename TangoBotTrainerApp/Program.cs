using TangoBotTrainerApi;
using TangoBotTrainerCoreLib;

class Program
{
    static void Main(string[] args)
    {
        // Resolve the Runtime
        IRuntime runtime = new Runtime();

        runtime.StartCampaign();
    }
}