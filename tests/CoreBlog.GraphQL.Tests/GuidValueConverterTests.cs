using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using GraphQL.Language.AST;
using GraphQL.Types;
using Xunit;

namespace CoreBlog.GraphQL.Tests {
    public class GuidValueConverterTests {
        private readonly GuidValueConverter _guidValueConverter;

        public GuidValueConverterTests() {
            _guidValueConverter = new GuidValueConverter();
        }

        [Fact] public void Convert_ShouldCreateGuidValueWrapper() {
            var guid = Guid.NewGuid();

            _guidValueConverter.Convert(guid, null)

                .Should().BeOfType<GuidValue>()
                
                .Which.Value.Should().Be(guid);
        }

        [Fact] public void Matches_ShouldReturnTrueForGuids() {
            _guidValueConverter.Matches(Guid.NewGuid(), null).Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(0)]
        [InlineData(0L)]
        public void Matches_ShouldReturnFalseForNonGuidTypes(object value) {
            _guidValueConverter.Matches(value, null).Should().BeFalse();
        }
    }
}
