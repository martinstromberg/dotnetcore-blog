using System;
using CoreBlog.Data.Abstractions.Posts;
using FluentAssertions;
using Xunit;

namespace CoreBlog.Data.Abstractions.Tests.Posts {
    public class IBlogPostExtensionMethodsTests {
        public class TestBlogPost : IBlogPost {
            public Guid BlogPostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime? Created { get; set; }
            public DateTime? LastUpdated { get; set; }
            public Guid AuthorId { get; set; }
        }

        [Fact]
        public void CopyTo_CopiesAllInterfaceData() {
            var original = new TestBlogPost {
                BlogPostId = Guid.NewGuid(),
                Title = "Hello, world!",
                Content = "This is a blog post",
                Created = DateTime.Parse("1970-01-01 00:00:00"),
                LastUpdated = DateTime.Parse("2000-01-01 00:00:00"),
                AuthorId = Guid.NewGuid()
            };

            original.CopyTo(new TestBlogPost()).Should().BeEquivalentTo(original, "otherwise we're missing fields");
        }
    }
}
