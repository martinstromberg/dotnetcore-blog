using System;
using CoreBlog.Data.Abstractions.Users;
using FluentAssertions;
using Xunit;

namespace CoreBlog.Data.Abstractions.Tests.Posts {
    public class IUserExtensionMethodsTests {
        public class TestUser : IUser {
            public Guid UserId { get; set; }
            public string EmailAddress { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public byte PasswordFormat { get; set; }
            public DateTime PasswordUpdated { get; set; }
        }

        [Fact]
        public void CopyTo_CopiesAllInterfaceData() {
            var original = new TestUser {
                UserId = Guid.NewGuid(),
                DisplayName = "Joe Smith",
                EmailAddress = "user@host.tld",
                Password = "changeme",
                PasswordFormat = 1,
                PasswordUpdated = new DateTime(2014, 7, 3)
            };

            original.CopyTo(new TestUser()).Should().BeEquivalentTo(original, "otherwise we're missing fields");
        }
    }
}
