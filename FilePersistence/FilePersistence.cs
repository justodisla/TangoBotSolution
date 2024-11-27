using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBotAPI.Persistence;

namespace FilePersistence
{
    public class FilePersistence<T> : IPersistence<T> where T : IEntity
    {
        private const string FILE_PERSISTENCE_FILE = "FileBasedPersistence";
        private readonly string _basePath;

        public FilePersistence()
        {
            _basePath = FILE_PERSISTENCE_FILE;

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<T> CreateAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            var table = await LoadTableAsync(tableName);
            var record = new Record
            {
                Id = entity.Id,
                Data = JsonSerializer.Serialize(entity)
            };

            table.Records.Add(record);
            await SaveTableAsync(table);
            return (T)entity;
        }

        public async Task<T> ReadAsync(Guid id)
        {
            foreach (var file in Directory.GetFiles(_basePath, "*.json"))
            {
                var table = await LoadTableAsync(Path.GetFileNameWithoutExtension(file));
                var record = table.Records.Find(r => r.Id == id);
                if (record != null)
                {
                    return JsonSerializer.Deserialize<T>(record.Data);
                }
            }

            return default(T);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            var entities = new List<T>();
            foreach (var file in Directory.GetFiles(_basePath, "*.json"))
            {
                var table = await LoadTableAsync(Path.GetFileNameWithoutExtension(file));
                foreach (var record in table.Records)
                {
                    entities.Add(JsonSerializer.Deserialize<T>(record.Data));
                }
            }
            return entities;
        }

        public async Task<T> UpdateAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            var table = await LoadTableAsync(tableName);
            var record = table.Records.Find(r => r.Id == entity.Id);
            if (record == null)
            {
                throw new Exception("Record not found.");
            }

            record.Data = JsonSerializer.Serialize(entity);
            await SaveTableAsync(table);
            return (T)entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            foreach (var file in Directory.GetFiles(_basePath, "*.json"))
            {
                var table = await LoadTableAsync(Path.GetFileNameWithoutExtension(file));
                var record = table.Records.Find(r => r.Id == id);
                if (record != null)
                {
                    table.Records.Remove(record);
                    await SaveTableAsync(table);
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await DeleteAsync(entity.Id);
        }

        public async Task<bool> RemoveTableAsync(string tableName)
        {
            var tablePath = GetTablePath(tableName);
            if (File.Exists(tablePath))
            {
                File.Delete(tablePath);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<string>> ListTablesAsync()
        {
            var tables = new List<string>();
            foreach (var file in Directory.GetFiles(_basePath, "*.json"))
            {
                tables.Add(Path.GetFileNameWithoutExtension(file));
            }
            return tables;
        }

        private string GetTablePath(string tableName) => Path.Combine(_basePath, $"{tableName}.json");

        private async Task<Table> LoadTableAsync(string tableName)
        {
            var tablePath = GetTablePath(tableName);
            if (!File.Exists(tablePath))
            {
                return new Table
                {
                    Name = tableName,
                    Description = string.Empty,
                    Records = new List<Record>()
                };
            }

            var json = await File.ReadAllTextAsync(tablePath);
            return JsonSerializer.Deserialize<Table>(json) ?? new Table
            {
                Name = tableName,
                Description = string.Empty,
                Records = new List<Record>()
            };
        }

        private async Task SaveTableAsync(Table table)
        {
            var tablePath = GetTablePath(table.Name);
            var json = JsonSerializer.Serialize(table);
            await File.WriteAllTextAsync(tablePath, json);
        }

        private class Table
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Record> Records { get; set; }
        }

        private class Record
        {
            public Guid Id { get; set; }
            public string Data { get; set; }
        }
    }
}
