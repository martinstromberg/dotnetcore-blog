using System;
using CoreBlog.Data.Abstractions.Users;

namespace CoreBlog.Grains.Users {
    public class IncomingUser : IUser {
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public byte PasswordFormat { get; set; }

        public DateTime PasswordUpdated { get; set; }
    }
}
