using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreBlog.Data.EntityFramework {
    using Abstractions.Posts;
    using Posts;

    public static class IServiceCollectionExtensions {
        public static IServiceCollection AddEntityFrameworkBloggingServices(
            this IServiceCollection services, IConfiguration configuration) {
                var stringComparer = StringComparer.InvariantCultureIgnoreCase;

                var databaseProvider = configuration["Database.Provider"];
                var connectionString = configuration.GetConnectionString("BlogingDatabase");
                
                return services
                
                    .AddDbContext<BloggingContext>(options => {
                        if (stringComparer.Equals(databaseProvider, "Sqlite")) {
                            options.UseSqlite(connectionString);
                        } else if (stringComparer.Equals(databaseProvider, "SqlServer")) {
                            options.UseSqlServer(connectionString);
                        } else {
                            options.UseInMemoryDatabase(connectionString);
                        }
                    })

                    .AddTransient<IBlogPostRepository, BlogPostRepository>();
            }
    }
}