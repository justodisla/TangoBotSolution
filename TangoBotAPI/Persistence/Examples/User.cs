using System;

namespace TangoBotAPI.Persistence.Examples
{
    public class User : AbstractEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public override bool Validate()
        {
            // Custom validation logic for User entity
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email))
            {
                return false;
            }
            return base.Validate();
        }

        public override void BeforeSave()
        {
            // Custom action before saving User entity
            Console.WriteLine("Performing actions before saving the user.");
        }

        public override void AfterSave()
        {
            // Custom action after saving User entity
            Console.WriteLine("Performing actions after saving the user.");
        }

        public override string GetEntityName()
        {
            return "User";
        }

        public override string GetDescription()
        {
            return "System users table";
        }

        public override string GetTableName()
        {
            return "Users";
        }
    }
}
