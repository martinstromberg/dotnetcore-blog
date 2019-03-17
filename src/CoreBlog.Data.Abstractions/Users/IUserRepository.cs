using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.Data.Abstractions.Users {
    public interface IUserRepository {
        Task<IUser> FindUserByIdAsync(Guid id);

        Task<IEnumerable<IUser>> Query();
    }
}