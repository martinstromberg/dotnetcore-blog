using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreBlog.Data.EntityFramework.Users
{
    using Data.Abstractions.Users;

    public class UserRepository : IUserRepository {
        private readonly BloggingContext _databaseContext;

        public UserRepository(BloggingContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IUser> FindUserByIdAsync(Guid id)
        {
            return await _databaseContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<IUser>> Query()
        {
            return await _databaseContext.Users.ToListAsync();
        }
    }
}