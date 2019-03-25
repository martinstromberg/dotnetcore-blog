using CoreBlog.GraphQL.Types;
using FluentAssertions;
using GraphQL.Types;
using Xunit;

namespace CoreBlog.GraphQL.Tests.Types {
    public class UserTypeTests {
        private readonly UserType _userType;

        public UserTypeTests() {
            _userType = new UserType();
        }

        [Fact] public void ShouldHaveIdFieldOfTypeGuidGraphType() {
            _userType.Fields

                .Should().Contain(f => f.Name == "id",
                    "because users needs to declare an id")

                .Which.Type.Should().Be<NonNullGraphType<GuidGraphType>>(
                    "because the id is of type Guid");
        }

        [Fact] public void ShouldHaveDisplayNameFieldOfTypeString() {
            _userType.Fields

                .Should().Contain(f => f.Name == "displayName",
                    "because users needs to declare a display name")

                .Which.Type.Should().Be<NonNullGraphType<StringGraphType>>(
                    "because the display name is of type string");
        }

        [Fact] public void ShouldHaveEmailAddressFieldOfTypeString() {
            _userType.Fields

                .Should().Contain(f => f.Name == "emailAddress",
                    "because users needs to declare email address")

                .Which.Type.Should().Be<NonNullGraphType<StringGraphType>>(
                    "because the email address is of type string");
        }
    }
}
