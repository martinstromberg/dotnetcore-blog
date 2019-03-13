using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains {
    using Data.Abstractions;
    using Abstractions.Posts;

    public class BlogPostGrain : Grain, IBlogPostGrain {
        public Task<IBlogPost> Find() {
            var blogPostId = this.GetPrimaryKey();

            return Task.FromResult((IBlogPost)null);
        }

        public Task Store(IBlogPost blogPost) {
            return Task.CompletedTask;
        }
    }
}