using GraphQL.Types;

namespace CoreBlog.GraphQL.Types {
    public class BlogPostInputType : InputObjectGraphType {
        public BlogPostInputType() {
            Name = "BlogPostInput";

            Field<StringGraphType>("title");
            Field<NonNullGraphType<StringGraphType>>("content");
            Field<NonNullGraphType<GuidGraphType>>("authorId");
        }
    }
}