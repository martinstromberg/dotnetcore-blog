using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CoreBlog.GraphQL {
    using Types;
    using Schema;

    public static class IServiceCollectionExtensions {
        public static IServiceCollection AddGraphQLServices(this IServiceCollection services) {
            // Types
            services.AddSingleton<GuidGraphType>();

            services.AddSingleton<BlogPostType>();
            services.AddSingleton<BlogPostInputType>();

            services.AddSingleton<UserType>();
            services.AddSingleton<TokenType>();

            // Query & Mutation
            services.AddSingleton<BlogQuery>();
            services.AddSingleton<BlogMutation>();

            // Schema
            services.AddSingleton<BlogSchema>();

            // Dependency Resolver
            services.AddSingleton<IDependencyResolver>(provider => 
                new FuncDependencyResolver(provider.GetRequiredService));

            return services;
        }
    }
}
