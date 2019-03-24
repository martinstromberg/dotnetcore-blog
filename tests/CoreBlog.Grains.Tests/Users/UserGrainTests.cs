using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreBlog.Data.Abstractions;
using CoreBlog.Data.Abstractions.Users;
using CoreBlog.GrainModels.Users;
using CoreBlog.Grains.Users;
using FluentAssertions;
using Moq;
using Orleans.TestKit;
using Xunit;

namespace CoreBlog.Grains.Tests.Users {
    public class UserGrainTests : TestKitBase {
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public UserGrainTests() {
            _unitOfWork = new Mock<IUnitOfWork>();

            Silo.AddService(_unitOfWork.Object);
        }

        [Fact]
        public void Find_ShouldReturnGrainModel() {
            var stored = new IncomingUser { UserId = Guid.NewGuid() };

            _unitOfWork.Setup(u => u.Users.Get(stored.UserId))
                .Returns(() => Task.FromResult<IUser>(stored));

            var grain = Silo.CreateGrain<UserGrain>(stored.UserId);

            grain.Find().GetAwaiter().GetResult().Should().BeEquivalentTo(stored.ToGrainModel(),
                "becuase that's what the UnitOfWork provided");

            _unitOfWork.Verify(u => u.Users.Get(stored.UserId), Times.Once(),
                "because we need to call it just once");

            _unitOfWork.VerifyNoOtherCalls();
        }

        [Fact]
        public void Update_ShouldCommitUnitOfWork() {
            var stored = new IncomingUser {
                UserId = Guid.NewGuid(),
                DisplayName = "Joe Bloggs"
            };

            _unitOfWork.Setup(u => u.Users.Get(stored.UserId))
                .Returns(() => Task.FromResult<IUser>(stored));

            _unitOfWork.Setup(u => u.Commit()).Returns(() => Task.CompletedTask);

            var update = new User {
                UserId = stored.UserId,
                DisplayName = "Joey"
            };

            Silo.CreateGrain<UserGrain>(stored.UserId)
                .Update(update).GetAwaiter().GetResult();

            stored.DisplayName.Should().Be(update.DisplayName);

            _unitOfWork.Verify(u => u.Users.Get(stored.UserId), Times.AtMostOnce(),
                "because need for fetch it 0 or 1 times");

            _unitOfWork.Verify(u => u.Commit(), Times.Once(),
                "because we need to commit our changes");

            _unitOfWork.VerifyNoOtherCalls();
        }
    }
}
