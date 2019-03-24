using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBlog.Data.Abstractions {
    public interface IWriteRepository<TEntity, TKey> where TEntity : class {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entities);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
