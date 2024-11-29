namespace TangoBot.API.Persistence
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

    }
}
