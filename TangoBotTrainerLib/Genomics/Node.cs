public enum NodeType
{
    Input,
    Hidden,
    Output
}

public class Node
{
    public int Id { get; private set; }
    public NodeType Type { get; private set; }
    public double Activation { get; set; }

    public Node(int id, NodeType type)
    {
        Id = id;
        Type = type;
        Activation = 0;
    }
}