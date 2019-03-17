namespace CoreBlog.Grains.Users
{
    using Data.Abstractions.Users;
    using GrainModels.Users;

    public static class IUserExtensionMethods {
        public static User ToGrainModel(this IUser user) {
            return new User {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                EmailAddress = user.EmailAddress,
            };
        }
    }
}