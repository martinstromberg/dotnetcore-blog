using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoreBlog.Data.EntityFramework {
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext> {
        public BloggingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
            optionsBuilder.UseSqlite("Data Source=../CoreBlog.SiloHost/blog.db");

            return new BloggingContext(optionsBuilder.Options);
        }
    }
}