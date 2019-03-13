using GraphQL.Types;
using System.Threading.Tasks;

namespace CoreBlog.GraphQL.Types {
    using Data.Abstractions;

    public class BlogPostType : ObjectGraphType<IBlogPost> {
        public BlogPostType() {
            Field<GuidGraphType>("id", resolve: context => Task.FromResult(context.Source.BlogPostId));
            Field(post => post.Title);
            Field(post => post.Content);
            Field(post => post.Created);
            Field<GuidGraphType>("authorId", resolve: context => Task.FromResult(context.Source.AuthorId));
        }
    }
}