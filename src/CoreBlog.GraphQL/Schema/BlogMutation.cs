using GraphQL.Types;
using Microsoft.Extensions.Configuration;

namespace CoreBlog.GraphQL.Schema {
    using GrainClientServices.Abstractions;

    public partial class BlogMutation : ObjectGraphType {
        protected readonly IBlogPostService _blogPostService;
        protected readonly IUserService _userService;
        protected readonly IConfiguration _configuration;

        public BlogMutation(IBlogPostService blogService, IUserService userService, IConfiguration configuration) {
            _blogPostService = blogService;
            _userService = userService;
            _configuration = configuration;

            SetupAuthenticationFields();
            SetupPostFields();
        }
    }
}