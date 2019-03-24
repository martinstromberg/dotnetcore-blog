using System;
using System.Collections.Generic;
using System.Text;
using CoreBlog.GrainModels.Posts;
using CoreBlog.Grains.Posts;
using FluentAssertions;
using Xunit;

namespace CoreBlog.Grains.Tests.Posts {
    public class BlogPostModelExtensionMethods {
        [Fact] public void ToGrainModel_ShouldTransferAllFields() {
            var original = new IncomingBlogPost();
            var expectation = new BlogPost();

            original.BlogPostId = expectation.BlogPostId = Guid.NewGuid();
            original.Title = expectation.Title = "Hello, world!";
            original.Content = expectation.Content = "This is a blog post";
            original.Created = expectation.Created = DateTime.UtcNow;
            original.AuthorId = expectation.AuthorId = Guid.NewGuid();

            original.ToGrainModel().Should().BeEquivalentTo(expectation,
                "because we need to be able to rely on having all fields transferred");
        }

        [Fact]
        public void ToDataModel_ShouldTransferAllFields() {
            var expectation = new IncomingBlogPost();
            var original = new BlogPost();

            original.BlogPostId = expectation.BlogPostId = Guid.NewGuid();
            original.Title = expectation.Title = "Hello, world!";
            original.Content = expectation.Content = "This is a blog post";
            expectation.Created = original.Created = DateTime.UtcNow;
            original.AuthorId = expectation.AuthorId = Guid.NewGuid();

            original.ToDataModel().Should().BeEquivalentTo(expectation,
                "because we need to be able to rely on having all fields transferred");
        }
    }
}
