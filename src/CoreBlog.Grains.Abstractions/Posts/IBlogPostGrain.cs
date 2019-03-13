using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Abstractions.Posts
{
    using Data.Abstractions;
    
    public interface IBlogPostGrain : IGrainWithGuidKey
    {
        Task<IBlogPost> Find();
        Task Store(IBlogPost blogPost);
    }
}
