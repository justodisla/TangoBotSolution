namespace TangoBotApi.Services.Persistence
{
    public interface IEntity
    {
        Guid Id { get; set; }
        object Data { get; set; }
    }
}
