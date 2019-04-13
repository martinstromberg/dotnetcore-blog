using Orleans;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Users {
    using Abstractions.Users;
    using Data.Abstractions;
    using GrainModels.Users;

    public class UserRepositoryGrain : Grain, IUserRepositoryGrain {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDictionary<string, Guid> _emailToIdMap;

        public UserRepositoryGrain(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;

            _emailToIdMap = new ConcurrentDictionary<string, Guid>();
        }

        public async Task<Guid> Add(User user) {
            var entity = user.ToDataModel();

            _unitOfWork.Users.Add(entity);
            await _unitOfWork.Commit();

            return entity.UserId;
        }

        public async Task<Guid> GetUserIdByEmailAddress(string emailAddress) {
            if (_emailToIdMap.ContainsKey(emailAddress)) {
                return _emailToIdMap[emailAddress];
            }

            var users = await _unitOfWork.Users.Find(u => u.EmailAddress == emailAddress);
            if (!users.Any()) {
                return Guid.Empty;
            }

            var user = users.First();
            if (user == null) {
                return Guid.Empty;
            }

            if (!_emailToIdMap.ContainsKey(emailAddress)) {
                _emailToIdMap.Add(user.EmailAddress, user.UserId);
            }

            return user.UserId;
        }

        public async Task<IEnumerable<User>> Query() {
            var users = (await _unitOfWork.Users.GetAll()).ToList();

            foreach (var user in users) {
                if (_emailToIdMap.ContainsKey(user.EmailAddress)) {
                    continue;
                }

                _emailToIdMap.Add(user.EmailAddress, user.UserId);
            }

            return users
                .Select(UserModelExtensionMethods.ToGrainModel)
                .ToList();
        }
    }
}
