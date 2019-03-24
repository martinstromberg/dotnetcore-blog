using System;

namespace CoreBlog.Data.Abstractions.Users {
    public interface IUserWriteRepository : IWriteRepository<IUser, Guid> {
    }
}
