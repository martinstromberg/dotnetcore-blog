using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace CoreBlog.Grains.Abstractions.Users {
    using GrainModels.Users;

    public interface IUserRepositoryGrain : IGrainWithIntegerKey {
        Task<IEnumerable<User>> Query();

        Task<Guid> GetUserIdByEmailAddress(string emailAddress);

        Task<Guid> Add(User user);
    }
}
