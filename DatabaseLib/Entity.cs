using TangoBotAPI.Persistence;

public class Entity : IEntity
{
    public Guid Id { get; set; }
    public bool Validate() => true;
    public void BeforeSave() { }
    public void AfterSave() { }

    public string GetEntityName()
    {
        return "Entity";
    }

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public string GetTableName()
    {
        throw new NotImplementedException();
    }
}
