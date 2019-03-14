using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlog.Grains {
    using GrainModels.Posts;
    using Abstractions.Posts;

    public class BlogPostRegistryGrain : Grain, IBlogPostRegistryGrain {
        private readonly System.Collections.Generic.ISet<BlogPost> _posts;

        public BlogPostRegistryGrain() {
            _posts = new System.Collections.Generic.HashSet<BlogPost>();
            _posts.Add(new BlogPost { Title = "Hello, world!", Content = "This is my first post." });
            _posts.Add(new BlogPost { Title = "Hello, Orleans!", Content = "This is my second post." });
            _posts.Add(new BlogPost { Title = "Hello, GraphQL!", Content = "This is my third post." });
        }

        public Task<IEnumerable<BlogPost>> Query() {
            var blogPostId = this.GetPrimaryKey();

            return Task.FromResult(_posts.AsEnumerable());
        }

        public Task<Guid> Add(BlogPost blogPost) {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}