using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreBlog.Data.EntityFramework.Users {
    using Data.Abstractions.Users;

    public class UserRepository : IUserRepository {
        private readonly BloggingContext _databaseContext;

        public UserRepository(BloggingContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Add(IUser user) {
            var entity = user.CopyTo(new User());

            entity.UserId = Guid.NewGuid();

            entity.CopyTo(user);

            _databaseContext.Users.Add(entity);
        }

        public void AddRange(IEnumerable<IUser> users) {
            foreach (var user in users) Add(user);
        }

        public async Task<IEnumerable<IUser>> Find(Expression<Func<IUser, bool>> predicate) {
            return await _databaseContext.Users
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IUser> Get(Guid id) {
            return await _databaseContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<IUser>> GetAll() {
            return await _databaseContext.Users.ToListAsync();
        }

        public void Remove(IUser user) {
            if (user is User userEntity) {
                _databaseContext.Users.Remove(userEntity);
                return;
            }

            if (user.UserId == Guid.Empty) {
                // TODO: I don't really know how I would deal with this... 
                // maybe throw something? Yeah, let's throw

                throw new InvalidOperationException("Unable to remove the user since it has no id");
            }

            userEntity = _databaseContext.Users.Find(user.UserId);

            _databaseContext.Users.Remove(userEntity);
        }

        public void RemoveRange(IEnumerable<IUser> users) {
            foreach (var user in users) Remove(user);
        }
    }
}