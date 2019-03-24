using System;
using System.Threading.Tasks;
using CoreBlog.GrainClientServices;
using CoreBlog.GrainModels.Posts;
using CoreBlog.Grains.Abstractions.Posts;
using FluentAssertions;
using Moq;
using Orleans;
using Xunit;

namespace CoreBlog.GrainClientServices.Tests {
    public class BlogPostServiceTests {
        private readonly BlogPostService _blogPostService;
        private Mock<IClusterClient> _clusterClient;

        public BlogPostServiceTests() {
            _clusterClient = new Mock<IClusterClient>();

            _blogPostService = new BlogPostService(_clusterClient.Object);
        }

        [Fact] public void GetPostByIdAsync_ShouldContactBlogPostGrain() {
            var postGrain = new Mock<IBlogPostGrain>();
            
            var post = new BlogPost { BlogPostId = Guid.NewGuid() };

            _clusterClient.Setup(c => c.GetGrain<IBlogPostGrain>(post.BlogPostId, null))
                .Returns(() => postGrain.Object);

            postGrain.Setup(g => g.Find()).Returns(() => Task.FromResult(post));

            _blogPostService.GetPostByIdAsync(post.BlogPostId).GetAwaiter().GetResult()
                .Should().Be(post, "because that's what we requested");

            _clusterClient.Verify(c => c.GetGrain<IBlogPostGrain>(post.BlogPostId, null), Times.Once(),
                "because we need to fetch the grain");

            postGrain.Verify(g => g.Find(), Times.Once(),
                "because we need to fetch the post");

            _clusterClient.VerifyNoOtherCalls();
            postGrain.VerifyNoOtherCalls();
        }

        [Fact] public void GetPostById_ShouldContactBlogPostGrain() {
            var postGrain = new Mock<IBlogPostGrain>();

            var post = new BlogPost { BlogPostId = Guid.NewGuid() };

            _clusterClient.Setup(c => c.GetGrain<IBlogPostGrain>(post.BlogPostId, null))
                .Returns(() => postGrain.Object);

            postGrain.Setup(g => g.Find()).Returns(() => Task.FromResult(post));

            _blogPostService.GetPostById(post.BlogPostId)
                .Should().Be(post, "because that's what we requested");

            _clusterClient.Verify(c => c.GetGrain<IBlogPostGrain>(post.BlogPostId, null), Times.Once(),
                "because we need to fetch the grain");

            postGrain.Verify(g => g.Find(), Times.Once(),
                "because we need to fetch the post");

            _clusterClient.VerifyNoOtherCalls();
            postGrain.VerifyNoOtherCalls();
        }

        [Fact] public void CreatePost_ShouldCallAddOnRegistryGrain() {
            var postGrain = new Mock<IBlogPostRegistryGrain>();

            var postId = Guid.NewGuid();
            var post = new BlogPost { };

            _clusterClient.Setup(c => c.GetGrain<IBlogPostRegistryGrain>(0, null))
                .Returns(() => postGrain.Object);

            postGrain.Setup(g => g.Add(post)).Returns(() => Task.FromResult(postId));

            _blogPostService.CreatePost(post).GetAwaiter().GetResult()
                .Should().Be(postId, "because that's what the grain returns");

            _clusterClient.Verify(c => c.GetGrain<IBlogPostRegistryGrain>(0, null), Times.Once,
                "because we only need one instance of the grain");

            postGrain.Verify(g => g.Add(post), Times.Once(), "because we only want to create it once");
        }
    }
}
