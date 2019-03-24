using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreBlog.Data.Abstractions {
    public interface IRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>, IWriteRepository<TEntity, TKey>
        where TEntity : class {
    }
}
