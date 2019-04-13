using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBlog.GrainClientServices {
    using Abstractions;
    using GrainModels.Users;
    using Grains.Abstractions.Users;

    public class UserService : IUserService {
        private readonly IClusterClient _clusterClient;
        
        public UserService(IClusterClient clusterClient) {
            _clusterClient = clusterClient;
        }

        public User Find(Guid id) {
            return FindAsync(id).Result;
        }
        public Task<User> FindAsync(Guid id) {
            var userGrain = _clusterClient.GetGrain<IUserGrain>(id);

            return userGrain.Find();
        }

        public Task<IEnumerable<User>> Query() {
            return null;
        }

        public async Task<User> ValidateCredentials(string emailAddress, string password) {
            var userId = await _clusterClient.GetGrain<IUserRepositoryGrain>(0)
                .GetUserIdByEmailAddress(emailAddress);

            if (userId == Guid.Empty) {
                return null;
            }

            var userGrain = _clusterClient.GetGrain<IUserGrain>(userId);

            if (!await userGrain.ValidatePassword(password)) {
                return null;
            }

            return await userGrain.Find();
        }
    }
}