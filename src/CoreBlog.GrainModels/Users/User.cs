using System;

namespace CoreBlog.GrainModels.Users {
    public class User {
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }
    }
}