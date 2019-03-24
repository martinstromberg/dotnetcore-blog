using System;

namespace CoreBlog.Data.Abstractions.Users {
    public static class IUserExtensionMethods {
        public static TType CopyTo<TType>(this IUser source, TType destination)  where TType : IUser {
            if (destination == null) {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.UserId = source.UserId;
            destination.DisplayName = source.DisplayName;
            destination.EmailAddress = source.EmailAddress;

            return destination;
        }
    }
}
