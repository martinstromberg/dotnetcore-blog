using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlog.GraphQL.Services {
    using Data.Abstractions;
    using Models;

    public interface IBlogPostService {
        IBlogPost GetPostById(Guid id);
        Task<IBlogPost> GetPostByIdAsync(Guid id);

        Task<IEnumerable<IBlogPost>> GetBlogPostsAsync();
    }

    public class BlogPostService : IBlogPostService
    {
        private readonly IList<IBlogPost> _blogPosts;

        public BlogPostService() {
            _blogPosts = new List<IBlogPost>();

            _blogPosts.Add(new BlogPost(Guid.NewGuid(), "Hello, world!", "This is my first post.", DateTime.Now, Guid.NewGuid()));
            _blogPosts.Add(new BlogPost(Guid.NewGuid(), "Hello, GraphQL!", "This is my second post.", DateTime.Now, Guid.NewGuid()));
            _blogPosts.Add(new BlogPost(Guid.NewGuid(), "Hello, dotnet!", "This is my third post.", DateTime.Now, Guid.NewGuid()));
        }

        public Task<IEnumerable<IBlogPost>> GetBlogPostsAsync()
        {
            return Task.FromResult(
                _blogPosts.AsEnumerable()
            );
        }

        public IBlogPost GetPostById(Guid id)
        {
            return GetPostByIdAsync(id).Result;
        }

        public Task<IBlogPost> GetPostByIdAsync(Guid id)
        {
            return Task.FromResult(
                _blogPosts.Single(p => Equals(id, p.BlogPostId))
            );
        }
    }
}