namespace TangoBotTrainerApi
{
    public interface IAgentInterface
    {
        string Name { get; }
        string Description { get; }
        Type Type { get; }
    }
}