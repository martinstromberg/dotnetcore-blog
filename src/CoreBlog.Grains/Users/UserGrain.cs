using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Users {
    using GrainModels.Users;
    using Abstractions.Users;

    public class UserGrain : Grain, IUserGrain {
        public Task<User> Find() {
            var userId = this.GetPrimaryKey();

            return Task.FromResult<User>(new User {
                UserId = userId,
                DisplayName = "Blogger Bob",
                EmailAddress = "bob@example-blog.local"
            });
        }

        public Task Update(User user) {
            return Task.CompletedTask;
        }
    }
}