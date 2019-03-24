using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreBlog.Data.EntityFramework {
    using Abstractions;
    using Abstractions.Posts;
    using Posts;
    using Abstractions.Users;
    using Users;

    public static class IServiceCollectionExtensions {
        public static IServiceCollection AddEntityFrameworkBloggingServices(
            this IServiceCollection services, IConfiguration configuration) {
                var stringComparer = StringComparer.InvariantCultureIgnoreCase;

                var databaseProvider = configuration["Database:Provider"];
                var connectionString = configuration.GetConnectionString("BloggingDatabase");
                
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

                    .AddTransient<IUnitOfWork, EfUnitOfWork>()

                    .AddTransient<IBlogPostReadRepository, BlogPostRepository>()
                    .AddTransient<IUserReadRepository, UserRepository>()
                ;
            }
    }
}