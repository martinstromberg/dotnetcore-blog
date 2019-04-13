using System;

namespace CoreBlog.Data.Abstractions.Users {
    public interface IUser {
        Guid UserId { get; set; }

        string EmailAddress { get; set; }

        string DisplayName { get; set; }

        string Password { get; set; }

        /// <summary>
        /// 1 = ClearText, 2 = Hash+Salt, 3 = External crypto
        /// </summary>
        byte PasswordFormat { get; set; } 

        DateTime PasswordUpdated { get; set; }
    }
}