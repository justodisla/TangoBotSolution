namespace TangoBot.API.Persistence
{

    public abstract class AbstractEntity : IEntity
    {
        public Guid Id { get; set; }

        // Default implementation of the Validate method
        public virtual bool Validate()
        {
            // Basic validation logic, can be overridden by derived classes
            return true;
        }

        // Default implementation of the BeforeSave method
        public virtual void BeforeSave()
        {
            // Default action before saving, can be overridden by derived classes
        }

        // Default implementation of the AfterSave method
        public virtual void AfterSave()
        {
            // Default action after saving, can be overridden by derived classes
        }

        public abstract string GetEntityName();

        public abstract string GetDescription();

        public abstract string GetTableName();
    }
}
