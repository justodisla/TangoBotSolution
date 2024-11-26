namespace TangoBotAPI.Persistence
{
    public interface IEntity
    {
        Guid Id { get; set; }

        // Method to validate the entity
        bool Validate();

        // Method to perform any entity-specific actions before saving
        void BeforeSave();

        // Method to perform any entity-specific actions after saving
        void AfterSave();

        // Method to get the name of the entity
        string GetEntityName();

        // Method to get the description of the entity
        string GetDescription();

        // Method to get the table name of the entity
        string GetTableName();
    }
}
