using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreBlog.Data.Abstractions {
    public interface IReadRepository<TEntity, TKey> where TEntity : class {
        Task<TEntity> Get(TKey id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
