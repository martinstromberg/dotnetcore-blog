using GraphQL;
using GraphQL.Types;
using GraphQlSchema = GraphQL.Types.Schema;

namespace CoreBlog.GraphQL.Schema {
    public class BlogSchema : GraphQlSchema {
        public BlogSchema(BlogQuery query, IDependencyResolver resolver) {
            Query = query;
            DependencyResolver = resolver;
        }
    }
}