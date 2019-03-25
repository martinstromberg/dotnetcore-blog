using System;
using System.Collections.Generic;
using System.Text;
using CoreBlog.GraphQL.Types;
using FluentAssertions;
using GraphQL.Types;
using Xunit;

namespace CoreBlog.GraphQL.Tests.Types {
    public class BlogPostInputTypeTests {
        private readonly BlogPostInputType _blogPostInputType;

        public BlogPostInputTypeTests() {
            _blogPostInputType = new BlogPostInputType();
        }

        [Fact] public void ShouldHaveNameBlogPostInput() {
            _blogPostInputType.Name.Should().Be("BlogPostInput");
        }

        [Fact] public void ShouldHaveTitleFieldOfTypeStringGraphType() {
            _blogPostInputType.Fields

                .Should().Contain(f => f.Name == "title")

                .Which.Type.Should().Be<StringGraphType>();
        }

        [Fact] public void ShouldHaveContentFieldOfTypeNonNullStringGraphType() {
            _blogPostInputType.Fields

                .Should().Contain(f => f.Name == "content")

                .Which.Type.Should().Be<NonNullGraphType<StringGraphType>>();
        }

        [Fact]public void ShouldHaveAuthorIdFieldOfTypeNonNullGuidGraphType() {
            _blogPostInputType.Fields

                .Should().Contain(f => f.Name == "authorId")

                .Which.Type.Should().Be<NonNullGraphType<GuidGraphType>>();
        }
    }
}
