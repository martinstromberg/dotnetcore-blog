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
    public class BlogPostRegistryGrainTests : TestKitBase {
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public BlogPostRegistryGrainTests() {
            _unitOfWork = new Mock<IUnitOfWork>();

            Silo.AddService(_unitOfWork.Object);
        }

        [Fact] public void Add_ShouldCommitANewPost() {
            var post = new BlogPost { Title = "Hello, world!" };

            _unitOfWork.Setup(u => u.Commit()).Returns(() => Task.CompletedTask);
            _unitOfWork.Setup(u => u.Posts.Add(It.Is<IBlogPost>(p => p.Title == post.Title)));

            Silo.CreateGrain<BlogPostRegistryGrain>(0).Add(post).GetAwaiter().GetResult();

            _unitOfWork.Verify(
                u => u.Posts.Add(It.Is<IBlogPost>(p => p.BlogPostId == post.BlogPostId)),
                Times.Once(),
                "because the post needs to be added"
            );

            _unitOfWork.Verify(u => u.Commit(), Times.Once(), 
                "because we need to commit the change");

            _unitOfWork.VerifyNoOtherCalls();
        }
    }
}
