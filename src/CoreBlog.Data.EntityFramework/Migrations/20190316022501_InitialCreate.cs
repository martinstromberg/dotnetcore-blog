using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreBlog.Data.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 265, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    PasswordFormat = table.Column<byte>(nullable: false),
                    PasswordUpdated = table.Column<DateTime>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    BlogPostId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Content = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    AuthorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.BlogPostId);
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DisplayName", "EmailAddress", "Password", "PasswordFormat", "PasswordUpdated" },
                values: new object[] { new Guid("23a9dbd8-474b-4bf3-a39e-f16dc0c078fc"), "Blogger Bob", "bob@blog.local", "changeme", (byte)1, new DateTime(2019, 3, 16, 2, 25, 0, 884, DateTimeKind.Utc).AddTicks(7170) });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "BlogPostId", "AuthorId", "Content", "Title" },
                values: new object[] { new Guid("be2f05f3-3b8c-4430-b649-1908eef23f7e"), new Guid("23a9dbd8-474b-4bf3-a39e-f16dc0c078fc"), "This is your first post.", "Hello, world!" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
