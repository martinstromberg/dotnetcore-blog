using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.GrainClientServices.Abstractions {
    using GrainModels;

    public interface IBlogPostService {
        BlogPost GetPostById(Guid id);
        Task<BlogPost> GetPostByIdAsync(Guid id);

        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();
    }
}