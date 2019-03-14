using GraphQL.Types;

namespace CoreBlog.GraphQL.Schema {
    using GrainClientServices.Abstractions;
    using Types;

    public class BlogQuery : ObjectGraphType<object> {
        public BlogQuery(IBlogPostService blogPosts) {
            Name = "Query";

            Field<ListGraphType<BlogPostType>>(
                "posts",
                resolve: context => blogPosts.GetBlogPostsAsync()
            );
        }
    }
}