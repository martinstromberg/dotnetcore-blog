using System;

namespace CoreBlog.Data.Abstractions.Posts {
    public interface IBlogPost {
        Guid BlogPostId { get; set; }

        string Title { get; set; }

        string Content { get; set; }

        DateTime? Created { get; set; }

        DateTime? LastUpdated { get; set; }

        Guid AuthorId { get; set; }
    }
}
