using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace CoreBlog.Data.EntityFramework.Tests.Users {
    using Abstractions.Users;
    using EntityFramework.Users;

    public class UserRepositoryTests {
        private readonly BloggingContext _inMemoryContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests() {
            _inMemoryContext = new BloggingContext(new DbContextOptionsBuilder<BloggingContext>()
                // We want a new InMemory database for each test
                .UseInMemoryDatabase(string.Concat("UserRepositoryTests", DateTime.UtcNow.Ticks))
                .Options);

            _userRepository = new UserRepository(_inMemoryContext);
        }

        [Fact]
        public void Get_ShouldReturnUserIfExists() {
            var storedUser = new User { UserId = Guid.NewGuid() };

            _inMemoryContext.Users.Add(storedUser);
            _inMemoryContext.SaveChanges();

            _userRepository.Get(storedUser.UserId).Result.Should().BeEquivalentTo(storedUser,
                "we expect to get the user we're looking for");
        }

        [Fact]
        public void GetAll_ShouldReturnAllUsers() {
            var users = new List<User> {
                new User { UserId = Guid.NewGuid() },
                new User { UserId = Guid.NewGuid() },
                new User { UserId = Guid.NewGuid() },
                new User { UserId = Guid.NewGuid() }
            };

            _inMemoryContext.Users.AddRange(users);
            _inMemoryContext.SaveChanges();

            _userRepository.GetAll().Result.Should().BeEquivalentTo(users,
                "because we expect to get all the users stored in the database");
        }

        [Fact]
        public void Find_ShouldReturnAllUsersMatchingPredicate() {
            var users = new List<User> {
                new User { UserId = Guid.NewGuid(), DisplayName = "Dennis Reynolds" },
                new User { UserId = Guid.NewGuid(), DisplayName = "Ronald MacDonald" },
                new User { UserId = Guid.NewGuid(), DisplayName = "Charlie Kelly" },
                new User { UserId = Guid.NewGuid(), DisplayName = "Frank Reynolds" },
                new User { UserId = Guid.NewGuid(), DisplayName = "Dee Reynolds" }
            };

            _inMemoryContext.Users.AddRange(users);
            _inMemoryContext.SaveChanges();

            Expression<Func<IUser, bool>> predicate = post =>
                post.DisplayName.Contains("Reynolds");

            var expected = new List<IUser> {
                users[0], users[3], users[4]
            };

            _userRepository.Find(predicate).Result.Should().BeEquivalentTo(expected,
                "we expect to get all users matching the predicate");
        }

        [Fact]
        public void Add_ShouldAddUser() {
            var post = new User { DisplayName = "Dayman" };

            _userRepository.Add(post);

            _inMemoryContext.ChangeTracker.Entries<User>().Should().HaveCount(1,
                "only one user was added");

            var entry = _inMemoryContext.ChangeTracker.Entries<User>().First();

            entry.State.Should().Be(EntityState.Added,
                "we haven't commited the changes yet");

            entry.Entity.Should().BeEquivalentTo(post,
                "it was what was added");
        }

        [Fact]
        public void AddRange_ShouldAddUsers() {
            var users = new List<IUser> {
                new User { DisplayName = "Dayman" },
                new User { DisplayName = "Nightman" },
                new User { DisplayName = "The Waitress" },
            };

            _userRepository.AddRange(users);

            _inMemoryContext.ChangeTracker.Entries<User>().Should().HaveCount(users.Count,
                "that's how many users were added");

            var entries = _inMemoryContext.ChangeTracker.Entries<User>();

            entries.Select(e => e.State).Should().AllBeEquivalentTo(EntityState.Added,
                "we haven't commited the changes yet");

            entries.Select(e => e.Entity).Should().BeEquivalentTo(users,
                "they were the users that were added");
        }

        [Fact]
        public void Remove_ShouldRemoveUser() {
            var user = new User { UserId = Guid.NewGuid(), DisplayName = "Cricket" };
            _inMemoryContext.Users.Add(user);
            _inMemoryContext.SaveChanges();

            _userRepository.Remove(user);

            _inMemoryContext.ChangeTracker.Entries<User>().Should().HaveCount(1,
                "only one user was removed");

            var entry = _inMemoryContext.ChangeTracker.Entries<User>().First();

            entry.State.Should().Be(EntityState.Deleted,
                "we haven't commited the changes yet");

            entry.Entity.Should().BeEquivalentTo(user,
                "it was what was removed");
        }

        [Fact]
        public void RemoveRange_ShouldRemoveUsers() {
            var users = new List<User> {
                new User { DisplayName = "Artemis" },
                new User { DisplayName = "Uncle Jack" },
                new User { DisplayName = "The Lawyer" },
            };

            _inMemoryContext.Users.AddRange(users);
            _inMemoryContext.SaveChanges();

            users = users.Skip(1).ToList();

            _userRepository.RemoveRange(users);

            var entries = _inMemoryContext.ChangeTracker.Entries<User>()
                .Where(e => e.State != EntityState.Unchanged);

            entries.Should().HaveCount(users.Count,
                "that's how many users were removed");

            entries.Select(e => e.State).Should().AllBeEquivalentTo(EntityState.Deleted,
                "we haven't commited the changes yet");

            entries.Select(e => e.Entity).Should().BeEquivalentTo(users,
                "they were the users that were removed");
        }
    }
}
