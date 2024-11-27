using System;

namespace TangoBotAPI.Persistence
{
    public static class EntityFactory
    {
        public static IEntity CreateEntity(string name, string description)
        {
            return new Entity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description
            };
        }

        private class Entity : IEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;

            public string GetTableName()
            {
                return Name;
            }

            public void BeforeSave()
            {
                // Implement logic to execute before saving the entity
            }

            public void AfterSave()
            {
                // Implement logic to execute after saving the entity
            }

            public string GetEntityName()
            {
                return Name;
            }

            public string GetDescription()
            {
                return Description;
            }

            public bool Validate()
            {
                throw new NotImplementedException();
            }
        }
    }
}
