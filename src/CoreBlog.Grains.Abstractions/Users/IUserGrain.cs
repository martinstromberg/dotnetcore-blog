using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Abstractions.Users
{   
    using GrainModels.Users;
    public interface IUserGrain : IGrainWithGuidKey {
        Task<User> Find();

        Task Update(User user);
    }
}