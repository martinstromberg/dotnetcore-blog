using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace CoreBlog.Grains.Users {
    using Abstractions.Users;
    using Data.Abstractions;
    using GrainModels.Users;

    public class UserGrain : Grain, IUserGrain {
        private readonly ILogger<UserGrain> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private User _userModel = null;

        public UserGrain(ILogger<UserGrain> logger, IUnitOfWork unitOfWork) {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Find() {
            return (_userModel = _userModel ?? (await _unitOfWork.Users.Get(this.GetPrimaryKey())).ToGrainModel());
        }

        public async Task Update(User user) {
            var entity = await _unitOfWork.Users.Get(this.GetPrimaryKey());

            entity.DisplayName = user.DisplayName;

            await _unitOfWork.Commit();
        }
    }
}