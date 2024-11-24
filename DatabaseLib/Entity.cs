using TangoBotAPI.Persistence;

public class Entity : IEntity
{
    public Guid Id { get; set; }
    public bool Validate() => true;
    public void BeforeSave() { }
    public void AfterSave() { }
}
