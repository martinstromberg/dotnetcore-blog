using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.Data.EntityFramework.Posts {
    using Abstractions.Posts;

    public class BlogPostRepository : IBlogPostRepository {
        private readonly BloggingContext _databaseContext;

        public BlogPostRepository(BloggingContext databaseContext) {
            _databaseContext = databaseContext;
        }

        public async Task<IBlogPost> FindPostByIdAsync(Guid id) {
            return await _databaseContext.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<IBlogPost>> Query() {
            return await _databaseContext.Posts.ToListAsync();
        }
    }
}