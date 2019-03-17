using GraphQL;
using GraphQL.Types;
using GraphQlSchema = GraphQL.Types.Schema;

namespace CoreBlog.GraphQL.Schema {
    public class BlogSchema : GraphQlSchema {
        public BlogSchema(IDependencyResolver resolver, BlogQuery query, BlogMutation mutation) {
            DependencyResolver = resolver;

            Query = query;
            Mutation = mutation;

            RegisterType<GuidGraphType>();
            RegisterValueConverter(new GuidValueConverter());
        }
    }
}