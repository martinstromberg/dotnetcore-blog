using System;

namespace CoreBlog.Grains.Posts {
    using Data.Abstractions.Posts;

    public class IncomingBlogPost : IBlogPost {
        public Guid BlogPostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastUpdated { get; set; }

        public Guid AuthorId { get; set; }
    }
}
