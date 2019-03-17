using Microsoft.EntityFrameworkCore;
using System;

namespace CoreBlog.Data.EntityFramework {
    using Posts;
    using Users;

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            ConfigureUsers(modelBuilder);
            ConfigureBlogPosts(modelBuilder);
        }

        protected void ConfigureUsers(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>(entity => {
                // Primary key
                entity.HasKey(post => post.UserId);

                // Field definitions
                entity
                    .Property(user => user.DisplayName)
                    .HasMaxLength(128)
                    .IsRequired();

                entity
                    .Property(user => user.EmailAddress)
                    .HasMaxLength(265)
                    .IsRequired();

                entity
                    .Property(user => user.Password)
                    .IsRequired();

                entity
                    .Property(user => user.PasswordFormat)
                    .IsRequired();

                entity
                    .Property(user => user.PasswordUpdated)
                    .IsRequired();

                // Foreign keys
                entity
                    .HasMany(user => user.AuthoredPosts)
                    .WithOne(post => post.Author)
                    .HasPrincipalKey(user => user.UserId)
                    .HasForeignKey(post => post.AuthorId);

                // Indexes
                entity
                    .HasIndex(user => user.EmailAddress)
                    .IsUnique();

                // Seed
                entity.HasData(
                    new User
                    {
                        UserId = Guid.Parse("23a9dbd8-474b-4bf3-a39e-f16dc0c078fc"),
                        DisplayName = "Blogger Bob",
                        EmailAddress = "bob@blog.local",
                        Password = "changeme",
                        PasswordFormat = 1,
                        PasswordUpdated = DateTime.UtcNow
                    }
                );
            });
        }

        protected void ConfigureBlogPosts(ModelBuilder modelBuilder) {
            // Primary key
            modelBuilder.Entity<BlogPost>()
                .HasKey(post => post.BlogPostId);

            // Field definitions
            modelBuilder.Entity<BlogPost>()
                .Property(post => post.Title)
                .HasMaxLength(256);

            modelBuilder.Entity<BlogPost>()
                .Property(post => post.Content)
                .IsRequired();

            modelBuilder.Entity<BlogPost>()
                .Property(post => post.Created)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BlogPost>()
                .Property(post => post.LastUpdated)
                .ValueGeneratedOnAddOrUpdate();

            // Foreign keys
            modelBuilder.Entity<BlogPost>()
                .HasOne(post => post.Author)
                .WithMany(user => user.AuthoredPosts)
                .HasForeignKey(post => post.AuthorId)
                .HasPrincipalKey(user => user.UserId);

            // Seed
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost { 
                    BlogPostId = Guid.Parse("be2f05f3-3b8c-4430-b649-1908eef23f7e"),
                    Title = "Hello, world!", 
                    Content = "This is your first post.",
                    AuthorId = Guid.Parse("23a9dbd8-474b-4bf3-a39e-f16dc0c078fc")
                }
            );
        }

        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
