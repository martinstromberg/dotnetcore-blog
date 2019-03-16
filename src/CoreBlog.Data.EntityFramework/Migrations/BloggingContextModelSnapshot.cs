﻿// <auto-generated />
using System;
using CoreBlog.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreBlog.Data.EntityFramework.Migrations
{
    [DbContext(typeof(BloggingContext))]
    partial class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity("CoreBlog.Data.EntityFramework.Posts.BlogPost", b =>
                {
                    b.Property<Guid>("BlogPostId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AuthorId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime?>("Created")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("BlogPostId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            BlogPostId = new Guid("be2f05f3-3b8c-4430-b649-1908eef23f7e"),
                            AuthorId = new Guid("23a9dbd8-474b-4bf3-a39e-f16dc0c078fc"),
                            Content = "This is your first post.",
                            Title = "Hello, world!"
                        });
                });

            modelBuilder.Entity("CoreBlog.Data.EntityFramework.Users.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(265);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<byte>("PasswordFormat");

                    b.Property<DateTime>("PasswordUpdated");

                    b.HasKey("UserId");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("23a9dbd8-474b-4bf3-a39e-f16dc0c078fc"),
                            DisplayName = "Blogger Bob",
                            EmailAddress = "bob@blog.local",
                            Password = "changeme",
                            PasswordFormat = (byte)1,
                            PasswordUpdated = new DateTime(2019, 3, 16, 2, 25, 0, 884, DateTimeKind.Utc).AddTicks(7170)
                        });
                });

            modelBuilder.Entity("CoreBlog.Data.EntityFramework.Posts.BlogPost", b =>
                {
                    b.HasOne("CoreBlog.Data.EntityFramework.Users.User", "Author")
                        .WithMany("AuthoredPosts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
