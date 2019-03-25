using GraphQL.Types;
using System.Threading.Tasks;

namespace CoreBlog.GraphQL.Types {
    using GrainModels.Posts;
    using GrainClientServices.Abstractions;

    public class BlogPostType : ObjectGraphType<BlogPost> {
        public BlogPostType(IUserService userService) {
            Field<NonNullGraphType<GuidGraphType>>("id", 
                resolve: context => Task.FromResult(context.Source.BlogPostId));

            Field("title", post => post.Title, true);

            Field("content", post => post.Content);

            Field< NonNullGraphType<DateTimeGraphType>>("created", resolve: context => context.Source.Created);

            Field<NonNullGraphType<GuidGraphType>>("authorId", resolve: context => Task.FromResult(context.Source.AuthorId));

            Field<NonNullGraphType<UserType>>("author", 
                resolve: context => userService.FindAsync(context.Source.AuthorId));
        }
    }
}