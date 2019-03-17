using GraphQL;
using GraphQL.Types;

namespace CoreBlog.GraphQL.Schema {
    using GrainClientServices.Abstractions;
    using GrainModels.Posts;
    using Types;

    public class BlogMutation : ObjectGraphType {
        public BlogMutation(IBlogPostService blogService, IUserService userService) {
            Field<BlogPostType>("createPost",
                description: "Creates a new blog post.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<BlogPostInputType>> { Name = "post"}
                ),
                resolve: context => {
                    var post = context.GetArgument<BlogPost>("post");

                    var task = blogService.CreatePost(post)
                        .ContinueWith(createTask => {
                            if (!createTask.IsCompletedSuccessfully) {
                                throw new ExecutionError("Unable to create blog post");
                            }

                            return blogService.GetPostByIdAsync(createTask.Result);
                        });

                    task.Wait();

                    if (!task.IsCompletedSuccessfully)
                    {
                        throw new ExecutionError("Unable to create blog post");
                    }

                    return task.Result;
                }
            );
        }
    }
}