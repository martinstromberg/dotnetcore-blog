using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Users {
    using Abstractions.Users;
    using Data.Abstractions.Users;
    using GrainModels.Users;

    public class UserGrain : Grain, IUserGrain {
        private readonly ILogger<UserGrain> _logger;
        private readonly IUserRepository _userRepository;

        private User _userModel = null;

        public UserGrain(ILogger<UserGrain> logger, IUserRepository userRepository) {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<User> Find() {
            return (_userModel = _userModel ?? (await _userRepository.FindUserByIdAsync(this.GetPrimaryKey())).ToGrainModel());
        }

        public Task Update(User user) {
            return Task.CompletedTask;
        }
    }
}