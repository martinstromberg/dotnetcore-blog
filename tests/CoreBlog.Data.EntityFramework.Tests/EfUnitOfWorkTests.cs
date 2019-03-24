using System;
using System.Linq;
using CoreBlog.Data.EntityFramework.Posts;
using CoreBlog.Data.EntityFramework.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreBlog.Data.EntityFramework.Tests {
    public class EfUnitOfWorkTests {
        private readonly BloggingContext _inMemoryContext;
        private readonly EfUnitOfWork _unitOfWork;

        public EfUnitOfWorkTests() {
            _inMemoryContext = new BloggingContext(new DbContextOptionsBuilder<BloggingContext>()
                // We want a new InMemory database for each test
                .UseInMemoryDatabase(string.Concat("UserRepositoryTests", DateTime.UtcNow.Ticks))
                .Options);

            _unitOfWork = new EfUnitOfWork(_inMemoryContext);
        }

        [Fact] public void Commit_ShouldMakeEntityFrameworkCommitPendingChanges() {
            _inMemoryContext.Posts.Add(new BlogPost { BlogPostId = Guid.NewGuid() });

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Select(e => e.State)
                .Should().AllBeEquivalentTo(EntityState.Added,
                    "we haven't commited anything yet");

            _unitOfWork.Commit().Wait();

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Select(e => e.State)
                .Should().AllBeEquivalentTo(EntityState.Unchanged,
                    "we called Commit()");
        }

        [Fact] public void Reject_ShouldUnstageAllPendingChanges() {
            _inMemoryContext.Posts.Add(new BlogPost { BlogPostId = Guid.NewGuid() });

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Select(e => e.State)
                .Should().AllBeEquivalentTo(EntityState.Added,
                    "we haven't commited anything yet");

            _unitOfWork.Reject();

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Select(e => e.State)
                .Should().AllBeEquivalentTo(EntityState.Unchanged,
                    "we called Reject()");
        }

        [Fact] public void Dispose_ShouldDisposeDbContext() {
            _unitOfWork.Dispose();

            Action action = () => _inMemoryContext.SaveChanges();

            action.Should().Throw<ObjectDisposedException>("because the DbContext has should have been disposed");
        }

        [Fact] public void Posts_ShouldReturnBlogPostRepository() {
            _unitOfWork.Posts.Should().BeOfType<BlogPostRepository>(
                "because it's our default repository for blog posts");
        }

        [Fact]
        public void Users_ShouldReturnUserRepository() {
            _unitOfWork.Users.Should().BeOfType<UserRepository>(
                "because it's our default repository for blog posts");
        }
    }
}
