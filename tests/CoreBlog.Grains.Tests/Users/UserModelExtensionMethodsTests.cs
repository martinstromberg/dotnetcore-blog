using System;
using System.Collections.Generic;
using System.Text;
using CoreBlog.GrainModels.Users;
using CoreBlog.Grains.Users;
using FluentAssertions;
using Xunit;

namespace CoreBlog.Grains.Tests.Users {
    public class UserModelExtensionMethodsTests {
        [Fact] public void ToGrainModel_ShouldTransferAllFields() {
            var original = new IncomingUser();
            var expected = new User();

            original.UserId = expected.UserId = Guid.NewGuid();
            original.DisplayName = expected.DisplayName = "Joe Bloggs";
            original.EmailAddress = expected.EmailAddress = "user@host.tld";

            original.ToGrainModel().Should().BeEquivalentTo(expected,
                "because we expect all fields to be transferred");
        }

        [Fact]
        public void ToDataModel_ShouldTransferAllFields() {
            var original = new User();
            var expected = new IncomingUser();

            original.UserId = expected.UserId = Guid.NewGuid();
            original.DisplayName = expected.DisplayName = "Joe Bloggs";
            original.EmailAddress = expected.EmailAddress = "user@host.tld";

            original.ToDataModel().Should().BeEquivalentTo(expected,
                "because we expect all fields to be transferred");
        }
    }
}
