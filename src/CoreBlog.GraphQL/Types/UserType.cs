using GraphQL.Types;
using System.Threading.Tasks;

namespace CoreBlog.GraphQL.Types {
    using GrainModels.Users;

    public class UserType : ObjectGraphType<User> {
        public UserType() {
            Field<NonNullGraphType<GuidGraphType>>("id", 
                resolve: context => Task.FromResult(context.Source.UserId));

            Field("displayName", u => u.DisplayName);

            Field("emailAddress", u => u.EmailAddress);
        }
    }
}