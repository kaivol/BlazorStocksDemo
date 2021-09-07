using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorIndexedDbJs;
using Microsoft.JSInterop;

namespace GitHubBlazor.Data
{
    // public class Portfolios : IDBObjectStore
    // {
    //     public IDBIndex Id { get; }
    //     public IDBIndex NameIndex { get; }
    //     public IDBIndex Assets { get; }
    //     
    //     public Portfolios(IDBDatabase idbDatabase) : base(idbDatabase)
    //     {
    //         Name = nameof(Portfolios);
    //         KeyPath = "id";
    //         AutoIncrement = true;
    //
    //         Id = new IDBIndex(this, "id", "id");
    //         NameIndex = new IDBIndex(this, "name", "name");
    //         Assets = new IDBIndex(this, "assets", "assets");
    //     }
    // }

    // ReSharper disable once InconsistentNaming
    public abstract class TypesafeIDBObjectStore<TData, TKey> : IDBObjectStore where TKey : notnull
    {
        protected TypesafeIDBObjectStore(IDBDatabase idbDatabase) : base(idbDatabase)
        {
            Name = nameof(TData);
            KeyPath = "id";
            AutoIncrement = true;
            foreach (var index in typeof(TData).GetProperties())
            {
                var _ = new IDBIndex(this, index.Name, index.Name);
            }
        }

        public Task<TData?> Get(TKey key) => Get<TKey, TData>(key);

        public Task<List<TData>> GetAll(int? count = null) => GetAll<TData>();
        
        public Task<TKey> Add(TData value) => Add<TData, TKey>(value);
        
        public Task<TKey> Put(TData data, TKey key) => base.Put(data, key);
        
        public Task Delete(TKey key) => base.Delete<TKey>(key);
        
#pragma warning disable 693
        [Obsolete("Use type-safe alternatives", true)]
        public new Task<TKey> Put<TData, TKey>(TData data, TKey key) => throw new InvalidOperationException();

        [Obsolete("Use type-safe alternatives", true)]
        public new Task Delete<TKey>(TKey key) where TKey : notnull => throw new InvalidOperationException();
#pragma warning restore 693
    }

    public class PortfoliosDB : TypesafeIDBObjectStore<PortfolioWithId, long>
    {
        public PortfoliosDB(IDBDatabase idbDatabase) : base(idbDatabase) { }
    }


    public class Database: IDBDatabase
    {
        public PortfoliosDB Portfolios { get; }

        public Database(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            Name = "Database";
            Version = 3;

            Portfolios = new PortfoliosDB(this);
        }
    }
}