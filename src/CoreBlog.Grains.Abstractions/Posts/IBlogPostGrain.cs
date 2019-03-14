using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Abstractions.Posts
{
    using GrainModels;
    
    public interface IBlogPostGrain : IGrainWithGuidKey
    {
        Task<BlogPost> Find();
        Task Store(BlogPost blogPost);
    }
}
