using System;
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

            Field<BlogPostType>(
                "post",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GuidGraphType>> {
                        Name = "id",
                        Description = "The ID of the post to fetch"
                    }
                ),
                resolve: context => blogPosts.GetPostByIdAsync(
                    context.GetArgument<Guid>("id")
                )
            );
        }
    }
}