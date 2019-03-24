using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBlog.Data.Abstractions.Users {
    public interface IUserReadRepository : IReadRepository<IUser, Guid> {
    }
}
