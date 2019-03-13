using System;

namespace CoreBlog.GraphQL.Models {
    using Data.Abstractions;
    
    public class BlogPost : IBlogPost {
        public BlogPost(Guid id, string title, string content, DateTime created, Guid authorId) {
            BlogPostId = id;
            Title = title;
            Content = content;
            Created = Created;
            AuthorId = authorId;
        }

        public Guid BlogPostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public Guid AuthorId { get; set; }
    }
}