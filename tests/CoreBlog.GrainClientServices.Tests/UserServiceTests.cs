using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreBlog.GrainModels.Users;
using CoreBlog.Grains.Abstractions.Users;
using FluentAssertions;
using Moq;
using Orleans;
using Xunit;

namespace CoreBlog.GrainClientServices.Tests {
    public class UserServiceTests {
        private readonly UserService _userService;
        private Mock<IClusterClient> _clusterClient;

        public UserServiceTests() {
            _clusterClient = new Mock<IClusterClient>();

            _userService = new UserService(_clusterClient.Object);
        }

        [Fact] public void Find_ShouldGetUserFromUserGrain() {
            var postGrain = new Mock<IUserGrain>();

            var user = new User { UserId = Guid.NewGuid() };

            _clusterClient.Setup(c => c.GetGrain<IUserGrain>(user.UserId, null))
                .Returns(() => postGrain.Object);

            postGrain.Setup(g => g.Find()).Returns(() => Task.FromResult(user));

            _userService.Find(user.UserId)
                .Should().Be(user, "because that's what we requested");

            _clusterClient.Verify(c => c.GetGrain<IUserGrain>(user.UserId, null), Times.Once(),
                "because we need to fetch the grain");

            postGrain.Verify(g => g.Find(), Times.Once(),
                "because we need to fetch the user");

            _clusterClient.VerifyNoOtherCalls();
            postGrain.VerifyNoOtherCalls();

        }

        [Fact] public void FindAsync_ShouldGetUserFromUserGrain() {
            var postGrain = new Mock<IUserGrain>();

            var user = new User { UserId = Guid.NewGuid() };

            _clusterClient.Setup(c => c.GetGrain<IUserGrain>(user.UserId, null))
                .Returns(() => postGrain.Object);

            postGrain.Setup(g => g.Find()).Returns(() => Task.FromResult(user));

            _userService.FindAsync(user.UserId).GetAwaiter().GetResult()
                .Should().Be(user, "because that's what we requested");

            _clusterClient.Verify(c => c.GetGrain<IUserGrain>(user.UserId, null), Times.Once(),
                "because we need to fetch the grain");

            postGrain.Verify(g => g.Find(), Times.Once(),
                "because we need to fetch the user");

            _clusterClient.VerifyNoOtherCalls();
            postGrain.VerifyNoOtherCalls();
        }

        [Fact]
        public void ValidateCredentials_ShouldLookUpUserIdByEmailAddress()
        {
            const string emailAddress = "username@host.local";
            const string password = "changeme";

            var repository = new Mock<IUserRepositoryGrain>();

            _clusterClient
                .Setup(c => c.GetGrain<IUserRepositoryGrain>(0, null))
                .Returns(repository.Object);

            repository
                .Setup(r => r.GetUserIdByEmailAddress(emailAddress))
                .Returns(Task.FromResult(Guid.Empty));

            _userService.ValidateCredentials(emailAddress, password).Wait();

            _clusterClient.Verify(c => c.GetGrain<IUserRepositoryGrain>(0, null), Times.Once);

            repository.Verify(r => r.GetUserIdByEmailAddress(emailAddress), Times.Once);

            repository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ValidateCredentials_ShouldReturnNullIfUserIdIsEmpty()
        {
            const string emailAddress = "username@host.local";
            const string password = "changeme";

            var repository = new Mock<IUserRepositoryGrain>();

            _clusterClient
                .Setup(c => c.GetGrain<IUserRepositoryGrain>(0, null))
                .Returns(repository.Object);

            repository
                .Setup(r => r.GetUserIdByEmailAddress(emailAddress))
                .Returns(Task.FromResult(Guid.Empty));

            _userService.ValidateCredentials(emailAddress, password)
                .GetAwaiter().GetResult()

                .Should().BeNull();

            _clusterClient.Verify(c => c.GetGrain<IUserRepositoryGrain>(0, null), Times.Once);

            repository.Verify(r => r.GetUserIdByEmailAddress(emailAddress), Times.Once);

            repository.VerifyNoOtherCalls();
        }

        [Fact] public void ValidateCredentials_ShouldCallValidateOnUserGrain()
        {
            var userId = Guid.NewGuid();
            const string emailAddress = "username@host.local";
            const string password = "changeme";

            var repository = new Mock<IUserRepositoryGrain>();
            var user = new Mock<IUserGrain>();

            _clusterClient
                .Setup(c => c.GetGrain<IUserRepositoryGrain>(0, null))
                .Returns(repository.Object);

            _clusterClient
                .Setup(c => c.GetGrain<IUserGrain>(userId, null))
                .Returns(user.Object);

            repository
                .Setup(r => r.GetUserIdByEmailAddress(emailAddress))
                .Returns(Task.FromResult(userId));

            user
                .Setup(u => u.ValidatePassword(password))
                .Returns(Task.FromResult(true));

            _userService.ValidateCredentials(emailAddress, password).Wait();

            _clusterClient.Verify(c => c.GetGrain<IUserRepositoryGrain>(0, null), Times.Once);

            _clusterClient.Verify(c => c.GetGrain<IUserGrain>(userId, null), Times.Once);

            repository.Verify(r => r.GetUserIdByEmailAddress(emailAddress), Times.Once);

            user.Verify(u => u.ValidatePassword(password), Times.Once);

            _clusterClient.VerifyNoOtherCalls();
            repository.VerifyNoOtherCalls();
        }

        [Fact] public void ValidateCredentials_ShouldReturnNullIfValidationFailed()
        {
            var userId = Guid.NewGuid();
            const string emailAddress = "username@host.local";
            const string password = "changeme";

            var repository = new Mock<IUserRepositoryGrain>();
            var user = new Mock<IUserGrain>();

            _clusterClient
                .Setup(c => c.GetGrain<IUserRepositoryGrain>(0, null))
                .Returns(repository.Object);

            _clusterClient
                .Setup(c => c.GetGrain<IUserGrain>(userId, null))
                .Returns(user.Object);

            repository
                .Setup(r => r.GetUserIdByEmailAddress(emailAddress))
                .Returns(Task.FromResult(userId));

            user
                .Setup(u => u.ValidatePassword(password))
                .Returns(Task.FromResult(false));

            _userService.ValidateCredentials(emailAddress, password)
                .GetAwaiter().GetResult()
                
                .Should().BeNull();

            _clusterClient.Verify(c => c.GetGrain<IUserRepositoryGrain>(0, null), Times.Once);

            _clusterClient.Verify(c => c.GetGrain<IUserGrain>(userId, null), Times.Once);

            repository.Verify(r => r.GetUserIdByEmailAddress(emailAddress), Times.Once);

            user.Verify(u => u.ValidatePassword(password), Times.Once);

            _clusterClient.VerifyNoOtherCalls();
            repository.VerifyNoOtherCalls();
        }

        [Fact] public void ValidateCredentials_ShouldReturnUserIfValidationSucceeded()
        {
            var userId = Guid.NewGuid();
            const string emailAddress = "username@host.local";
            const string password = "changeme";

            var userObject = new User {
                UserId = userId,
                EmailAddress = emailAddress
            };

            var repository = new Mock<IUserRepositoryGrain>();
            var user = new Mock<IUserGrain>();

            _clusterClient
                .Setup(c => c.GetGrain<IUserRepositoryGrain>(0, null))
                .Returns(repository.Object);

            _clusterClient
                .Setup(c => c.GetGrain<IUserGrain>(userId, null))
                .Returns(user.Object);

            repository
                .Setup(r => r.GetUserIdByEmailAddress(emailAddress))
                .Returns(Task.FromResult(userId));

            user
                .Setup(u => u.ValidatePassword(password))
                .Returns(Task.FromResult(true));

            user
                .Setup(u => u.Find())
                .Returns(Task.FromResult(userObject));

            _userService.ValidateCredentials(emailAddress, password)
                .GetAwaiter().GetResult()

                .Should().Be(userObject);

            _clusterClient.Verify(c => c.GetGrain<IUserRepositoryGrain>(0, null), Times.Once);

            _clusterClient.Verify(c => c.GetGrain<IUserGrain>(userId, null), Times.Once);

            repository.Verify(r => r.GetUserIdByEmailAddress(emailAddress), Times.Once);

            user.Verify(u => u.ValidatePassword(password), Times.Once);
            user.Verify(u => u.Find(), Times.Once);

            _clusterClient.VerifyNoOtherCalls();
            repository.VerifyNoOtherCalls();
            user.VerifyNoOtherCalls();
        }
    }
}
