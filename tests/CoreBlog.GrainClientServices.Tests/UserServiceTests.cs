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
    }
}
