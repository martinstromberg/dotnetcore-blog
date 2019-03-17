using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Posts {
    using Abstractions.Posts;
    using Data.Abstractions.Posts;
    using GrainModels.Posts;

    public class BlogPostRegistryGrain : Grain, IBlogPostRegistryGrain {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostRegistryGrain(IBlogPostRepository blogPostRepository) {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<IEnumerable<BlogPost>> Query() {
            return (await _blogPostRepository.Query())
                .Select(ModelExtensionMethods.ToGrainModel)
                .ToList();
        }

        public Task<Guid> Add(BlogPost blogPost) {
            return _blogPostRepository
                .Add(blogPost.ToDataModel());
        }
    }
}