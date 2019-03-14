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
    }
}