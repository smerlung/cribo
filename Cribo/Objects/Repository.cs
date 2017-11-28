namespace Cribo.Objects
{
    using Cribo.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Repository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        private Dictionary<TKey, TEntity> store { get; set; }

        public Repository(Dictionary<TKey, TEntity> store)
        {
            this.store = store;
        }

        public IEnumerable<TEntity> ReadAll()
        {
            return this.store.Values;
        }

        public TEntity Read(TKey key)
        {
            return this.store[key];
        }

        public IEnumerable<TEntity> ReadWhere(Func<TEntity, bool> where)
        {
            return this.store.Values.Where(where);
        }

        public void Create(TEntity entity)
        {
            if (this.store.ContainsKey(entity.GetKey()))
            {
                throw new Exception(string.Format("An entity with key {0} already exists", entity.GetKey()));
            }

            this.store.Add(entity.GetKey(), entity);
        }

        public void Update(TEntity entity)
        {
            if (!this.store.ContainsKey(entity.GetKey()))
            {
                throw new Exception(string.Format("No entity with key {0}", entity.GetKey()));
            }

            this.store[entity.GetKey()] = entity;
        }

        public void Delete(TEntity entity)
        {
            if (!this.store.ContainsKey(entity.GetKey()))
            {
                throw new Exception(string.Format("No entity with key {0}", entity.GetKey()));
            }

            this.store.Remove(entity.GetKey());
        }
    }
}
