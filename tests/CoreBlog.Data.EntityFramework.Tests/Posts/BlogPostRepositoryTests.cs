using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace CoreBlog.Data.EntityFramework.Tests.Posts {
    using Abstractions.Posts;
    using EntityFramework.Posts;

    public class BlogPostRepositoryTests {
        private readonly BloggingContext _inMemoryContext;
        private readonly BlogPostRepository _blogPostRepository;

        public BlogPostRepositoryTests() {
            _inMemoryContext = new BloggingContext(new DbContextOptionsBuilder<BloggingContext>()
                // We want a new InMemory database for each test
                .UseInMemoryDatabase(string.Concat("BlogPostRepositoryTests", DateTime.UtcNow.Ticks))
                .Options);

            _blogPostRepository = new BlogPostRepository(_inMemoryContext);
        }

        [Fact]
        public void Get_ShouldReturnBlogPostIfExists() {
            var storedPost = new BlogPost { BlogPostId = Guid.NewGuid() };
            _inMemoryContext.Posts.Add(storedPost);
            _inMemoryContext.SaveChanges();

            _blogPostRepository.Get(storedPost.BlogPostId).Result.Should().BeEquivalentTo(storedPost, 
                "because we expect to get the post we're looking for");
        }

        [Fact]
        public void GetAll_ShouldReturnAllPosts() {
            var posts = new List<BlogPost> {
                new BlogPost { BlogPostId = Guid.NewGuid() },
                new BlogPost { BlogPostId = Guid.NewGuid() },
                new BlogPost { BlogPostId = Guid.NewGuid() },
                new BlogPost { BlogPostId = Guid.NewGuid() }
            };

            _inMemoryContext.Posts.AddRange(posts);
            _inMemoryContext.SaveChanges();

            var expected = posts.Select(p => (IBlogPost)p).ToList();

            _blogPostRepository.GetAll().Result.Should().BeEquivalentTo(expected,
                "because we expect to get all the posts stored in the database");
        }

        [Fact]
        public void Find_ShouldReturnAllPostsMatchingPredicate() {
            var posts = new List<BlogPost> {
                new BlogPost { BlogPostId = Guid.NewGuid(), Title = "The First Post" },
                new BlogPost { BlogPostId = Guid.NewGuid(), Title = "The Second Post" },
                new BlogPost { BlogPostId = Guid.NewGuid(), Title = "The Third Post" },
                new BlogPost { BlogPostId = Guid.NewGuid(), Title = "The Fourth Post" }
            };

            _inMemoryContext.Posts.AddRange(posts);
            _inMemoryContext.SaveChanges();

            Expression<Func<IBlogPost, bool>> predicate = post => 
                post.Title.Contains("ir");

            var expected = new List<IBlogPost> {
                posts[0], posts[2]
            };

            _blogPostRepository.Find(predicate).Result.Should().BeEquivalentTo(expected,
                "because we expect to get all the posts stored in the database");
        }

        [Fact]
        public void Add_ShouldAddPost() {
            var post = new BlogPost { Title = "Hello, xUnit" };

            _blogPostRepository.Add(post);

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Should().HaveCount(1,
                "only one post was added");

            var entry = _inMemoryContext.ChangeTracker.Entries<BlogPost>().First();

            entry.State.Should().Be(EntityState.Added,
                "we haven't commited the changes yet");

            entry.Entity.Should().BeEquivalentTo(post, 
                "it was what was added");
        }

        [Fact]
        public void AddRange_ShouldAddPosts() {
            var posts = new List<IBlogPost> {
                new BlogPost { Title = "Hello, xUnit" },
                new BlogPost { Title = "Hello, Tests" },
                new BlogPost { Title = "Hello, World" },
            };

            _blogPostRepository.AddRange(posts);

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Should().HaveCount(posts.Count,
                "that's how many posts were added");

            var entries = _inMemoryContext.ChangeTracker.Entries<BlogPost>();

            entries.Select(e => e.State).Should().AllBeEquivalentTo(EntityState.Added,
                "we haven't commited the changes yet");

            entries.Select(e => e.Entity).Should().BeEquivalentTo(posts,
                "they were the posts that were added");
        }

        [Fact]
        public void Remove_ShouldRemovePost() {
            var post = new BlogPost { BlogPostId = Guid.NewGuid(), Title = "Hello, xUnit" };
            _inMemoryContext.Posts.Add(post);
            _inMemoryContext.SaveChanges();

            _blogPostRepository.Remove(post);

            _inMemoryContext.ChangeTracker.Entries<BlogPost>().Should().HaveCount(1,
                "only one post was removed");

            var entry = _inMemoryContext.ChangeTracker.Entries<BlogPost>().First();

            entry.State.Should().Be(EntityState.Deleted,
                "we haven't commited the changes yet");

            entry.Entity.Should().BeEquivalentTo(post,
                "it was what was removed");
        }

        [Fact]
        public void RemoveRange_ShouldRemovePosts() {
            var posts = new List<BlogPost> {
                new BlogPost { Title = "Hello, xUnit" },
                new BlogPost { Title = "Hello, Tests" },
                new BlogPost { Title = "Hello, World" },
            };

            _inMemoryContext.Posts.AddRange(posts);
            _inMemoryContext.SaveChanges();

            posts = posts.Skip(1).ToList();

            _blogPostRepository.RemoveRange(posts);

            var entries = _inMemoryContext.ChangeTracker.Entries<BlogPost>()
                .Where(e => e.State != EntityState.Unchanged);

            entries.Should().HaveCount(posts.Count,
                "that's how many posts were removed");

            entries.Select(e => e.State).Should().AllBeEquivalentTo(EntityState.Deleted,
                "we haven't commited the changes yet");

            entries.Select(e => e.Entity).Should().BeEquivalentTo(posts,
                "they were the posts that were removed");
        }
    }
}
