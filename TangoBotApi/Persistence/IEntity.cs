namespace TangoBotApi.Persistence
{
    public interface IEntity
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
