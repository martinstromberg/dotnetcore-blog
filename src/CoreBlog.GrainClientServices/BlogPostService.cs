using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.GrainClientServices {
    using Abstractions;
    using GrainModels.Posts;
    using Grains.Abstractions.Posts;

    public class BlogPostService : IBlogPostService {
        private readonly IClusterClient _clusterClient;

        public Task<IEnumerable<BlogPost>> GetBlogPostsAsync() {
            var blogPostRegistry = _clusterClient
                .GetGrain<IBlogPostRegistryGrain>(0);

            return blogPostRegistry.Query();
        }

        public BlogPostService(IClusterClient clusterClient) {
            _clusterClient = clusterClient;
        }

        public BlogPost GetPostById(Guid id) {
            return GetPostByIdAsync(id).Result;
        }

        public Task<BlogPost> GetPostByIdAsync(Guid id) {
            var blogPost = _clusterClient
                .GetGrain<IBlogPostGrain>(id);

            return blogPost.Find();
        }

        public async Task<Guid> CreatePost(BlogPost post) {
            var registry = _clusterClient
                .GetGrain<IBlogPostRegistryGrain>(0);

            return await registry.Add(post);
        }
    }
}
