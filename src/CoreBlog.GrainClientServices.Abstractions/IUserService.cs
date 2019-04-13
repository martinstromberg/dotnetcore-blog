using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.GrainClientServices.Abstractions {
    using GrainModels.Users;

    public interface IUserService {
        User Find(Guid id);
        Task<User> FindAsync(Guid id);

        Task<IEnumerable<User>> Query();

        Task<User> ValidateCredentials(string emailAddress, string password);
    }
}