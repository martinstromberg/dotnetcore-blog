using GraphQL.Types;

namespace CoreBlog.GraphQL.Schema {
    using GrainClientServices.Abstractions;
    using Types;

    public class BlogPostsQuery : ObjectGraphType<object> {
        public BlogPostsQuery(IBlogPostService blogPosts) {
            Name = "Query";
            Field<ListGraphType<BlogPostType>>(
                "posts",
                resolve: context => blogPosts.GetBlogPostsAsync()
            );
        }
    }
}