public class Connection
{
    public int SourceNodeId { get; private set; }
    public int TargetNodeId { get; private set; }
    public double Weight { get; set; }
    public bool IsEnabled { get; set; }

    public Connection(int sourceNodeId, int targetNodeId, double weight, bool isEnabled = true)
    {
        SourceNodeId = sourceNodeId;
        TargetNodeId = targetNodeId;
        Weight = weight;
        IsEnabled = isEnabled;
    }
}