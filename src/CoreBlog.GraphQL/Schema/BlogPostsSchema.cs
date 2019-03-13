using GraphQL;
using GraphQL.Types;
using GraphQlSchema = GraphQL.Types.Schema;

namespace CoreBlog.GraphQL.Schema {
    public class BlogPostsSchema : GraphQlSchema {
        public BlogPostsSchema(BlogPostsQuery query, IDependencyResolver resolver) {
            Query = query;
            DependencyResolver = resolver;
        }
    }
}