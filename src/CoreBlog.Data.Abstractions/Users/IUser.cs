using System;

namespace CoreBlog.Data.Abstractions.Users {
    public interface IUser {
        Guid UserId { get; set; }

        string EmailAddress { get; set; }

        string DisplayName { get; set; }
    }
}