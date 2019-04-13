using CoreBlog.Data.Abstractions;
using CoreBlog.Data.Abstractions.Users;
using CoreBlog.GrainModels.Users;
using CoreBlog.Grains.Users;
using FluentAssertions;
using Moq;
using Orleans.TestKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace CoreBlog.Grains.Tests.Users
{
    public class UserRepositoryGrainTests : TestKitBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public UserRepositoryGrainTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();

            Silo.AddService(_unitOfWork.Object);
        }

        [Fact] public void GetUserIdByEmailAddress_ShouldLookupUserIfNotCached()
        {
            const string emailAddress = "username@host.local";

            _unitOfWork
                .Setup(u => u.Users.Find(It.IsAny<Expression<Func<IUser, bool>>>()))
                .Returns(Task.FromResult<IEnumerable<IUser>>(new List<IUser>()));

            Silo.CreateGrain<UserRepositoryGrain>(0)
                .GetUserIdByEmailAddress(emailAddress)
                .Wait();

            _unitOfWork.Verify(
                u => u.Users.Find(It.IsAny<Expression<Func<IUser, bool>>>()),
                Times.Once);

            _unitOfWork.VerifyNoOtherCalls();
        }

        [Fact] public void GetUserIdByEmailAddress_ShouldReturnEmptyIfEmailAddressUnknown()
        {
            const string emailAddress = "username@host.local";

            _unitOfWork
                .Setup(u => u.Users.Find(It.IsAny<Expression<Func<IUser, bool>>>()))
                .Returns(Task.FromResult<IEnumerable<IUser>>(new List<IUser>()));

            Silo.CreateGrain<UserRepositoryGrain>(0)
                .GetUserIdByEmailAddress(emailAddress)
                .GetAwaiter().GetResult()

                .Should().BeEmpty();
        }

        [Fact] public void GetUserIdByEmailAddress_ShouldReturnUserIdForFirstUserFound()
        {
            const string emailAddress = "username@host.local";

            var users = new List<IUser>
            {
                new IncomingUser { EmailAddress = emailAddress, UserId = Guid.NewGuid() },
                new IncomingUser { EmailAddress = emailAddress, UserId = Guid.NewGuid() },
                new IncomingUser { EmailAddress = emailAddress, UserId = Guid.NewGuid() },
            };

            _unitOfWork
                .Setup(u => u.Users.Find(It.IsAny<Expression<Func<IUser, bool>>>()))
                .Returns(Task.FromResult<IEnumerable<IUser>>(users));

            Silo.CreateGrain<UserRepositoryGrain>(0)
                .GetUserIdByEmailAddress(emailAddress)
                .GetAwaiter().GetResult()

                .Should().Be(users[0].UserId);
        }

        [Fact] public void Add_ShouldCommitANewUser()
        {
            var user = new User { EmailAddress = "username@host.local" };

            _unitOfWork.Setup(u => u.Commit()).Returns(() => Task.CompletedTask);
            _unitOfWork.Setup(w => w.Users.Add(It.Is<IUser>(u => u.EmailAddress == user.EmailAddress)));

            Silo.CreateGrain<UserRepositoryGrain>(0).Add(user).GetAwaiter().GetResult();

            _unitOfWork.Verify(
                w => w.Users.Add(It.Is<IUser>(u => u.EmailAddress == user.EmailAddress)),
                Times.Once(),
                "because the post needs to be added"
            );

            _unitOfWork.Verify(w => w.Commit(), Times.Once(),
                "because we need to commit the change");

            _unitOfWork.VerifyNoOtherCalls();
        }
    }
}
