using GraphQL.Types;

namespace CoreBlog.GraphQL.Schema {
    using Services;
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