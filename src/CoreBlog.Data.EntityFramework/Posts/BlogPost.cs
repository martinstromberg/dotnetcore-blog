using System;

namespace CoreBlog.Data.EntityFramework.Posts {
    using Abstractions.Posts;
    using Users;

    public class BlogPost : IBlogPost {
        public Guid BlogPostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastUpdated { get; set; }

        public Guid AuthorId { get; set; }

        public User Author { get; set; }
    }
}