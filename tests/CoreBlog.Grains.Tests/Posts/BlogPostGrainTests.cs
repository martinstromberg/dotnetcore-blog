using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreBlog.Data.Abstractions;
using CoreBlog.Data.Abstractions.Posts;
using CoreBlog.GrainModels.Posts;
using CoreBlog.Grains.Posts;
using FluentAssertions;
using Moq;
using Orleans.TestKit;
using Xunit;

namespace CoreBlog.Grains.Tests.Posts {
    public class BlogPostGrainTests : TestKitBase {
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public BlogPostGrainTests() {
            _unitOfWork = new Mock<IUnitOfWork>();

            Silo.AddService(_unitOfWork.Object);
        }

        [Fact] public void Find_ShouldReturnGrainModel() {
            var stored = new IncomingBlogPost { BlogPostId = Guid.NewGuid() };

            _unitOfWork.Setup(u => u.Posts.Get(stored.BlogPostId))
                .Returns(() => Task.FromResult<IBlogPost>(stored));

            var grain = Silo.CreateGrain<BlogPostGrain>(stored.BlogPostId);

            grain.Find().GetAwaiter().GetResult().Should().BeEquivalentTo(stored.ToGrainModel(), 
                "becuase that's what the UnitOfWork provided");

            _unitOfWork.Verify(u => u.Posts.Get(stored.BlogPostId), Times.Once(),
                "because we need to call it just once");

            _unitOfWork.VerifyNoOtherCalls();
        }

        [Fact] public void Update_ShouldCommitUnitOfWork() {
            var stored = new IncomingBlogPost {
                BlogPostId = Guid.NewGuid(),
                Title = "Hello, world!"
            };

            _unitOfWork.Setup(u => u.Posts.Get(stored.BlogPostId))
                .Returns(() => Task.FromResult<IBlogPost>(stored));

            _unitOfWork.Setup(u => u.Commit()).Returns(() => Task.CompletedTask);

            var update = new BlogPost {
                BlogPostId = stored.BlogPostId,
                Title = "Hello, xUnit!"
            };

            Silo.CreateGrain<BlogPostGrain>(stored.BlogPostId)
                .Update(update).GetAwaiter().GetResult();

            stored.Title.Should().Be(update.Title);

            _unitOfWork.Verify(u => u.Posts.Get(stored.BlogPostId), Times.AtMostOnce(),
                "because need for fetch it 0 or 1 times");

            _unitOfWork.Verify(u => u.Commit(), Times.Once(),
                "because we need to commit our changes");

            _unitOfWork.VerifyNoOtherCalls();
        }
    }
}
