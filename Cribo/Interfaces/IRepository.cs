namespace Cribo.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IRepository<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        TEntity Read(TKey key);

        IEnumerable<TEntity> ReadAll();

        IEnumerable<TEntity> ReadWhere(Func<TEntity, bool> where);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
