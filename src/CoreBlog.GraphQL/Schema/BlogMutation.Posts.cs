using GraphQL;
using GraphQL.Types;

namespace CoreBlog.GraphQL.Schema {
    using GrainModels.Posts;
    using Types;

    public partial class BlogMutation {
        public void SetupPostFields() {
            Field<BlogPostType>("createPost",
                description: "Creates a new blog post.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<BlogPostInputType>> { Name = "post" }
                ),
                resolve: ResolvePostMutation
            );
        }

        public object ResolvePostMutation(ResolveFieldContext<object> context) {
            var userContext = context.UserContext as BlogUserContext;
            if (userContext == null) {
                throw new ExecutionError("An error occurred");
            }

            if (!userContext.IsAuthenticated) {
                throw new ExecutionError("User is not authenticated. Please check the bearer token in the Authorization header.");
            }

            var post = context.GetArgument<BlogPost>("post");

            var task = _blogPostService.CreatePost(post)
                .ContinueWith(createTask => {
                    if (!createTask.IsCompletedSuccessfully) {
                        throw new ExecutionError("Unable to create blog post");
                    }

                    return _blogPostService.GetPostByIdAsync(createTask.Result);
                });

            task.Wait();

            if (!task.IsCompletedSuccessfully) {
                throw new ExecutionError("Unable to create blog post");
            }

            return task.Result;

        }
    }
}
