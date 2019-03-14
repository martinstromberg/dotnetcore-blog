using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Abstractions.Posts
{
    using GrainModels.Posts;
    
    public interface IBlogPostRegistryGrain : IGrainWithIntegerKey
    {
        Task<IEnumerable<BlogPost>> Query();

        Task<Guid> Add(BlogPost post);
    }
}
