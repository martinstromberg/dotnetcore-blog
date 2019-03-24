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
        }

        [Fact]
        public void CopyTo_CopiesAllInterfaceData() {
            var original = new TestUser {
                UserId = Guid.NewGuid(),
                DisplayName = "Joe Smith",
                EmailAddress = "user@host.tld"
            };

            original.CopyTo(new TestUser()).Should().BeEquivalentTo(original, "otherwise we're missing fields");
        }
    }
}
