namespace CoreBlog.GraphQL {
    using GrainModels.Users;

    public class BlogUserContext {
        public BlogUserContext(bool isAuthenticated, User user = null) {
            IsAuthenticated = isAuthenticated;
            User = user;
        }

        public bool IsAuthenticated { get;  }
        public User User { get; }
    }
}
