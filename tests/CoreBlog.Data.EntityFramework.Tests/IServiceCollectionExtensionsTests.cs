using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace CoreBlog.Data.EntityFramework.Tests {
    using Data.Abstractions;
    using Data.Abstractions.Posts;
    using Data.Abstractions.Users;
    using Data.EntityFramework;
    using Data.EntityFramework.Posts;
    using Data.EntityFramework.Users;

    public class IServiceCollectionExtensionsTests {
        ServiceCollection services;

        public IServiceCollectionExtensionsTests() {
            services = new ServiceCollection();
        }

        [Fact]
        public void AddEntityFrameworkBloggingServices_Adds_Blogging_Context() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["ConnectionStrings:BloggingDatabase"] = "AddEntityFrameworkBloggingServices_Adds_Blogging_Context"
                })
                .Build();

            var foo = services
                .AddEntityFrameworkBloggingServices(configuration)
                .BuildServiceProvider()
                .GetService<BloggingContext>()
                .Should().NotBeNull().And.BeOfType<BloggingContext>();
        }

        [Fact]
        public void AddEntityFrameworkBloggingServices_Adds_IUnitOfWork_Implementation() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["ConnectionStrings:BloggingDatabase"] = "AddEntityFrameworkBloggingServices_Adds_IBlogPostRepository_Implementation"
                })
                .Build();

            var foo = services
                .AddEntityFrameworkBloggingServices(configuration)
                .BuildServiceProvider()
                .GetService<IUnitOfWork>()
                .Should().NotBeNull().And.BeOfType<EfUnitOfWork>();
        }

        [Fact]
        public void AddEntityFrameworkBloggingServices_Adds_IBlogPostReadRepository_Implementation() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["ConnectionStrings:BloggingDatabase"] = "AddEntityFrameworkBloggingServices_Adds_IBlogPostRepository_Implementation"
                })
                .Build();

            var foo = services
                .AddEntityFrameworkBloggingServices(configuration)
                .BuildServiceProvider()
                .GetService<IBlogPostReadRepository>()
                .Should().NotBeNull().And.BeOfType<BlogPostRepository>();
        }

        [Fact]
        public void AddEntityFrameworkBloggingServices_Adds_IUserReadRepository_Implementation() {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["ConnectionStrings:BloggingDatabase"] = "AddEntityFrameworkBloggingServices_Adds_IUserRepository_Implementation"
                })
                .Build();

            var foo = services
                .AddEntityFrameworkBloggingServices(configuration)
                .BuildServiceProvider()
                .GetService<IUserReadRepository>()
                .Should().NotBeNull().And.BeOfType<UserRepository>();
        }
    }
}
