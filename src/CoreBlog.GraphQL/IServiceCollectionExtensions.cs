using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CoreBlog.GraphQL {
    using Types;
    using Schema;

    public static class IServiceCollectionExtensions {
        public static IServiceCollection AddGraphQLServices(this IServiceCollection services) {
            // Types
            services.AddScoped<GuidGraphType>();
            services.AddScoped<BlogPostType>();
            services.AddScoped<UserType>();

            // Query
            services.AddScoped<BlogQuery>();

            // Schema
            services.AddScoped<BlogSchema>();

            // Dependency Resolver
            services.AddScoped<IDependencyResolver>(provider => 
                new FuncDependencyResolver(provider.GetRequiredService));

            return services;
        }
    }
}
