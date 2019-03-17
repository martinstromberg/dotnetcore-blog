using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.GrainClientServices.Abstractions {
    using GrainModels.Posts;

    public interface IBlogPostService {
        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();

        BlogPost GetPostById(Guid id);
        Task<BlogPost> GetPostByIdAsync(Guid id);

        Task<Guid> CreatePost(BlogPost post);
    }
}