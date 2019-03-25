using CoreBlog.GrainClientServices.Abstractions;
using CoreBlog.GraphQL.Types;
using FluentAssertions;
using GraphQL.Types;
using Moq;
using Xunit;

namespace CoreBlog.GraphQL.Tests.Types {
    public class BlogPostTypeTests {
        private readonly Mock<IUserService> _userService;
        private readonly BlogPostType _blogPostType;

        public BlogPostTypeTests() {
            _userService = new Mock<IUserService>();
            _blogPostType = new BlogPostType(_userService.Object);
        }

        [Fact] public void ShouldHaveIdFieldOfTypeGuidGraphType() {
            _blogPostType.Fields

                .Should().Contain(f => f.Name == "id",
                    "because blog posts needs to declare an id")

                .Which.Type.Should().Be<NonNullGraphType<GuidGraphType>>(
                    "because the id is of type Guid");
        }

        [Fact] public void ShouldHaveTitleFieldOfTypeString() {
            _blogPostType.Fields

                .Should().Contain(f => f.Name == "title",
                    "because blog posts needs to declare a title")

                .Which.Type.Should().Be<StringGraphType>(
                    "because the title is of type string");
        }

        [Fact] public void ShouldHaveContentFieldOfTypeString() {
            _blogPostType.Fields

                .Should().Contain(f => f.Name == "content",
                    "because blog posts needs to declare content")

                .Which.Type.Should().Be<NonNullGraphType<StringGraphType>>(
                    "because the content is of type string");
        }

        [Fact] public void ShouldHaveCreatedFieldOfTypeDateTime() {
            _blogPostType.Fields

                .Should().Contain(f => f.Name == "created",
                    "because blog posts needs to declare content")

                .Which.Type.Should().Be<NonNullGraphType<DateTimeGraphType>>(
                    "because the created field is of type DateTime");
        }

        [Fact] public void ShouldHaveAuthorIdFieldOfTypeGuidGraphType() {
            _blogPostType.Fields

                .Should().Contain(f => f.Name == "authorId",
                    "because blog posts needs to declare an authorId")

                .Which.Type.Should().Be<NonNullGraphType<GuidGraphType>>(
                    "because the id is of type Guid");
        }

        [Fact] public void ShouldHaveAuthorFieldOfTypeUserType() {
            _blogPostType.Fields

                .Should().Contain(f => f.Name == "author",
                    "because blog posts needs to declare an author")

                .Which.Type.Should().Be<NonNullGraphType<UserType>>(
                    "because the id is of type User");
        }
    }
}
