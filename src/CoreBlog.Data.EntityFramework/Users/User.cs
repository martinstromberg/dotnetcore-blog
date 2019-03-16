using System;
using System.Collections.Generic;

namespace CoreBlog.Data.EntityFramework.Users {
    using Abstractions.Users;
    using Posts;

    public class User : IUser {
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public byte PasswordFormat { get; set; } // 1 = ClearText, 2 = Hash+Salt, 3 = External crypto

        public DateTime PasswordUpdated { get; set; }

        public string DisplayName { get; set; }

        public List<BlogPost> AuthoredPosts { get; set; }
    }
}