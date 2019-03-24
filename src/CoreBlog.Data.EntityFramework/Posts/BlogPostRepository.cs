using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreBlog.Data.EntityFramework.Posts {
    using Abstractions.Posts;

    public class BlogPostRepository : IBlogPostRepository {
        private readonly BloggingContext _databaseContext;

        public BlogPostRepository(BloggingContext databaseContext) {
            _databaseContext = databaseContext;
        }

        public void Add(IBlogPost post)
        {
            var entity = post.CopyTo(new BlogPost());

            entity.BlogPostId = Guid.NewGuid();
            entity.Created = DateTime.UtcNow;
            entity.LastUpdated = DateTime.UtcNow;

            entity.CopyTo(post);

            _databaseContext.Posts.Add(entity);
        }

        public void AddRange(IEnumerable<IBlogPost> posts) {
            foreach (var post in posts) Add(post);
        }

        public void Remove(IBlogPost post) {
            if (post is BlogPost postEntity) {
                _databaseContext.Posts.Remove(postEntity);
                return;
            }

            if (post.BlogPostId == Guid.Empty) {
                // TODO: I don't really know how I would deal with this... 
                // maybe throw something? Yeah, let's throw

                throw new InvalidOperationException("Unable to remove the post since it has no id");
            }

            postEntity = _databaseContext.Posts.Find(post.BlogPostId);

            _databaseContext.Posts.Remove(postEntity);
        }

        public void RemoveRange(IEnumerable<IBlogPost> posts) {
            foreach (var post in posts) Remove(post);
        }

        public async Task<IBlogPost> Get(Guid id) {
            return await _databaseContext.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<IBlogPost>> GetAll() {
            return await _databaseContext.Posts.ToListAsync();
        }

        public async Task<IEnumerable<IBlogPost>> Find(Expression<Func<IBlogPost, bool>> predicate) {
            return await _databaseContext.Posts.Where(predicate).ToListAsync();
        }
    }
}