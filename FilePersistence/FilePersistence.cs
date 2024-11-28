using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBotAPI.Persistence;

namespace FilePersistence
{
    public class FilePersistence : IPersistence
    {
        private const string FILE_PERSISTENCE_FILE = "FileBasedPersistence";
        private readonly string _basePath;

        public FilePersistence()
        {
            _basePath = FILE_PERSISTENCE_FILE;

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<TangoBotAPI.Persistence.ICollection<T>> GetCollectionAsync<T>(string collectionName) where T : IEntity
        {
            EnsureTableExists(collectionName);
            return new FileCollection<T>(_basePath, collectionName);
        }

        public async Task<IEnumerable<string>> ListCollectionsAsync()
        {
            var tables = new List<string>();
            foreach (var file in Directory.GetFiles(_basePath, "*.json"))
            {
                tables.Add(Path.GetFileNameWithoutExtension(file));
            }
            return tables;
        }

        public Task<bool> CreateCollectionAsync<T>(string collectionName) where T : IEntity
        {
            EnsureTableExists(collectionName);
            return Task.FromResult(true);
        }

        public async Task<bool> RemoveCollectionAsync(string collectionName)
        {
            var tablePath = GetTablePath(collectionName);
            if (File.Exists(tablePath))
            {
                File.Delete(tablePath);
                return true;
            }
            return false;
        }

        private void EnsureTableExists(string tableName)
        {
            var tablePath = GetTablePath(tableName);
            if (!File.Exists(tablePath))
            {
                var table = new Table
                {
                    Name = tableName,
                    Description = string.Empty,
                    Records = new List<Record>()
                };
                SaveTableAsync(table).Wait();
            }
        }

        private string GetTablePath(string tableName) => Path.Combine(_basePath, $"{tableName}.json");

        private async Task SaveTableAsync(Table table)
        {
            var tablePath = GetTablePath(table.Name);
            var json = JsonSerializer.Serialize(table);
            await File.WriteAllTextAsync(tablePath, json);
        }

        private class FileCollection<T> : TangoBotAPI.Persistence.ICollection<T> where T : IEntity
        {
            private readonly string _basePath;
            private readonly string _tableName;

            public FileCollection(string basePath, string tableName)
            {
                _basePath = basePath;
                _tableName = tableName;
            }

            public async Task<T> CreateAsync(T entity)
            {
                var table = await LoadTableAsync();
                var record = new Record
                {
                    Id = entity.Id,
                    Data = JsonSerializer.Serialize(entity)
                };

                table.Records.Add(record);
                await SaveTableAsync(table);
                return entity;
            }

            public async Task<T?> ReadAsync(Guid id)
            {
                var table = await LoadTableAsync();
                var record = table.Records.Find(r => r.Id == id);
                if (record != null)
                {
                    return JsonSerializer.Deserialize<T>(record.Data);
                }

                return default;
            }

            public async Task<IEnumerable<T>> ReadAllAsync()
            {
                var entities = new List<T>();
                var table = await LoadTableAsync();
                foreach (var record in table.Records)
                {
                    entities.Add(JsonSerializer.Deserialize<T>(record.Data));
                }
                return entities;
            }

            public async Task<T> UpdateAsync(T entity)
            {
                var table = await LoadTableAsync();
                var record = table.Records.Find(r => r.Id == entity.Id);
                if (record == null)
                {
                    throw new Exception("Record not found.");
                }

                record.Data = JsonSerializer.Serialize(entity);
                await SaveTableAsync(table);
                return entity;
            }

            public async Task<bool> DeleteAsync(Guid id)
            {
                var table = await LoadTableAsync();
                var record = table.Records.Find(r => r.Id == id);
                if (record != null)
                {
                    table.Records.Remove(record);
                    await SaveTableAsync(table);
                    return true;
                }

                return false;
            }

            public async Task<bool> DeleteAsync(T entity)
            {
                return await DeleteAsync(entity.Id);
            }

            private string GetTablePath() => Path.Combine(_basePath, $"{_tableName}.json");

            private async Task<Table> LoadTableAsync()
            {
                var tablePath = GetTablePath();
                if (!File.Exists(tablePath))
                {
                    return new Table
                    {
                        Name = _tableName,
                        Description = string.Empty,
                        Records = new List<Record>()
                    };
                }

                var json = await File.ReadAllTextAsync(tablePath);
                return JsonSerializer.Deserialize<Table>(json) ?? new Table
                {
                    Name = _tableName,
                    Description = string.Empty,
                    Records = new List<Record>()
                };
            }

            private async Task SaveTableAsync(Table table)
            {
                var tablePath = GetTablePath();
                var json = JsonSerializer.Serialize(table);
                await File.WriteAllTextAsync(tablePath, json);
            }
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
