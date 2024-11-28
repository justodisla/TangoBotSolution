﻿namespace TangoBotAPI.Persistence
{
    public interface IPersistence
    {
        Task<TangoBotAPI.Persistence.ICollection<T>> GetCollectionAsync<T>(string collectionName) where T : IEntity;
        Task<IEnumerable<string>> ListCollectionsAsync();
        Task<bool> CreateCollectionAsync<T>(string collectionName) where T : IEntity;
        Task<bool> RemoveCollectionAsync(string collectionName);
        void Setup(Dictionary<string, object> conf);
    }
}
