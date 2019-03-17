using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.Data.Abstractions.Posts {
    public interface IBlogPostRepository {
        Task<IBlogPost> FindPostByIdAsync(Guid id);

        Task<IEnumerable<IBlogPost>> Query();

        Task<Guid> Add(IBlogPost post);
    }
}