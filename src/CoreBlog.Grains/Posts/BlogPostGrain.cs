using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains {
    using GrainModels;
    using Abstractions.Posts;

    public class BlogPostGrain : Grain, IBlogPostGrain {
        public Task<BlogPost> Find() {
            var blogPostId = this.GetPrimaryKey();

            return Task.FromResult<BlogPost>(null);
        }

        public Task Store(BlogPost blogPost) {
            return Task.CompletedTask;
        }
    }
}