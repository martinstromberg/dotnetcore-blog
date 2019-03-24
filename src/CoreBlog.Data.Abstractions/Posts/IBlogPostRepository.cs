using System;

namespace CoreBlog.Data.Abstractions.Posts {
    public interface IBlogPostRepository : IRepository<IBlogPost, Guid>, IBlogPostReadRepository, IBlogPostWriteRepository {
    }
}