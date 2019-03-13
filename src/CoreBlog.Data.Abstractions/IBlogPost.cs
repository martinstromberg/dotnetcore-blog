using System;

namespace CoreBlog.Data.Abstractions
{
    public interface IBlogPost
    {
        Guid BlogPostId { get; set; }

        string Title { get; set; }

        string Content { get; set; }

        DateTime Created { get; set; }

        Guid AuthorId { get; set; }
    }
}
