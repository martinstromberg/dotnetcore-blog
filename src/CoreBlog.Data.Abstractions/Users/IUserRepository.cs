using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.Data.Abstractions.Users {
    public interface IUserRepository : IRepository<IUser, Guid>, IUserReadRepository, IUserWriteRepository {
    }
}