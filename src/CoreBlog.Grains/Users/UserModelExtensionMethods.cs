namespace CoreBlog.Grains.Users
{
    using Data.Abstractions.Users;
    using GrainModels.Users;

    public static class UserModelExtensionMethods {
        public static User ToGrainModel(this IUser user) {
            return new User {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                EmailAddress = user.EmailAddress,
            };
        }

        public static IUser ToDataModel(this User user) {
            return new IncomingUser {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                EmailAddress = user.EmailAddress,
            };
        }
    }
}