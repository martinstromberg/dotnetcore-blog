using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Posts {
    using Abstractions.Posts;
    using Data.Abstractions.Posts;
    using GrainModels.Posts;

    public class BlogPostGrain : Grain, IBlogPostGrain {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostGrain(IBlogPostRepository blogPostRepository) {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<BlogPost> Find() {
            var blogPost = await _blogPostRepository.FindPostByIdAsync(this.GetPrimaryKey());

            return blogPost.ToGrainModel();
        }

        public Task Update(BlogPost blogPost) {
            return Task.CompletedTask;
        }
    }
}