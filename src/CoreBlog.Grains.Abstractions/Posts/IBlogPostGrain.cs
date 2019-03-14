using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Abstractions.Posts
{
    using GrainModels.Posts;
    
    public interface IBlogPostGrain : IGrainWithGuidKey
    {
        Task<BlogPost> Find();
        Task Update(BlogPost blogPost);
    }
}
