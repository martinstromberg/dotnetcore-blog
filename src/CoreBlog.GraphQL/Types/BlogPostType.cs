using GraphQL.Types;
using System.Threading.Tasks;

namespace CoreBlog.GraphQL.Types {
    using GrainModels.Posts;
    using GrainClientServices.Abstractions;

    public class BlogPostType : ObjectGraphType<BlogPost> {
        public BlogPostType(IUserService userService) {
            Field<GuidGraphType>("id", resolve: context => Task.FromResult(context.Source.BlogPostId));
            Field(post => post.Title);
            Field(post => post.Content);
            Field(post => post.Created);
            Field<GuidGraphType>("authorId", resolve: context => Task.FromResult(context.Source.AuthorId));
            Field<UserType>("author", resolve: context => userService.FindAsync(context.Source.AuthorId));
        }
    }
}