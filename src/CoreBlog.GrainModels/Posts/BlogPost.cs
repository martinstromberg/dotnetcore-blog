using System;

namespace CoreBlog.GrainModels {
    public class BlogPost {
        public Guid BlogPostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public Guid AuthorId { get; set; }
    }
}