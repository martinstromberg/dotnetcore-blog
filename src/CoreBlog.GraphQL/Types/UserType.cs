using GraphQL.Types;
using System.Threading.Tasks;

namespace CoreBlog.GraphQL.Types {
    using GrainModels.Users;

    public class UserType : ObjectGraphType<User> {
        public UserType() {
            Field<GuidGraphType>("id", resolve: context => Task.FromResult(context.Source.UserId));
            Field(u => u.DisplayName);
            Field(u => u.EmailAddress);
        }
    }
}